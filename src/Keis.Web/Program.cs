using FluentValidation.AspNetCore;
using Keis.Data.Repository;
using Keis.Infrastructure;
using Keis.Infrastructure.Consumers.ReadEmail;
using Keis.Infrastructure.Consumers.UserCreatedEmail;
using Keis.Model;
using Keis.Model.Domain;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Quartz;
using System.Net.Mime;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
var bearerKey = builder.Configuration.GetSection("SecurityKey:Bearer").Value;

// Add services to the container.
builder.Services.Configure<ConnectionStringOptions>(builder.Configuration.GetSection("ConnectionStrings"));
builder.Services.Configure<SecurityKeyOptions>(builder.Configuration.GetSection("SecurityKey"));
builder.Services.AddControllersWithViews(o => o.Filters.Add<ValidationFilter>()).AddJsonOptions(o =>
{
    //o.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
}).AddFluentValidation(f =>
    f.RegisterValidatorsFromAssemblyContaining<IKeisInfrastructure>(includeInternalTypes: true));

builder.Services.AddMediatR(typeof(IKeisInfrastructure));
builder.Services.AddAutoMapper(typeof(ModelProfiles));
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// auth
builder.Services.AddIdentity<AuthUser, AuthRole>(o =>
{
    o.Password.RequireDigit = false;
    o.Password.RequireNonAlphanumeric = false;
    o.Password.RequireUppercase = false;
}).AddDefaultTokenProviders();
builder.Services
    .AddAuthentication(o =>
    {
        o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(o =>
    {
        o.SaveToken = true;
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(bearerKey)),
            ValidateIssuer = false,
            ValidateAudience = false,
            RequireExpirationTime = true,
            ValidateLifetime = true
        };
    });

builder.Services
    .AddAuthorization(o => o.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build());
builder.Services.AddTransient<IUserStore<AuthUser>, AuthUserRepository>();
builder.Services.AddTransient<IRoleStore<AuthRole>, AuthRoleRepository>();

// repositories
builder.Services.AddSingleton<IAuthUserRepository, AuthUserRepository>();
builder.Services.AddSingleton<ITicketRepository, TicketRepository>();
builder.Services.AddSingleton<IManageRepository, ManageRepository>();
builder.Services.AddSingleton<IAgentRepository, AgentReposiory>();

// Quartz configuration
builder.Services.AddQuartz(q =>
{
    q.SchedulerId = "__quartz__id__";
    q.UseMicrosoftDependencyInjectionJobFactory();

    q.UseSimpleTypeLoader();
    q.UseInMemoryStore();
    q.UseDefaultThreadPool(tp => { tp.MaxConcurrency = 4; });

    q.ScheduleJob<SupportEmailConsumer>(trigger
        => trigger.WithIdentity("__support_email_reader__").StartNow()
            .WithSimpleSchedule(s => s.WithIntervalInMinutes(5).RepeatForever())
            .WithDescription("Read support emails every 5 minutes and create a ticker if new email is available"));

    q.ScheduleJob<UserCreatedEmailConsumer>(trigger
        => trigger.WithIdentity("__user_created_emailer_").StartNow()
            .WithSimpleSchedule(s => s.WithIntervalInMinutes(5).RepeatForever())
            .WithDescription("Send email to newly created agent"));
});

builder.Services.AddQuartzServer(options =>
{
    // when shutting down we want jobs to complete gracefully
    options.WaitForJobsToComplete = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();

app.UseHttpsRedirection();
app.UseStatusCodePages(async statusCodeContext =>
{
    // using static System.Net.Mime.MediaTypeNames;
    statusCodeContext.HttpContext.Response.ContentType = MediaTypeNames.Application.Json;

    var codeMsg = statusCodeContext.HttpContext.Response.StatusCode switch
    {
        401 => "Unauthorized request",
        _ => ""
    };

    var unAuth = JsonSerializer.Serialize(new ErrorResult
    {
        Ok = false,
        Errors = new[] { codeMsg }
    });

    await statusCodeContext.HttpContext.Response.WriteAsync(unAuth);
});
app.UseStaticFiles();
app.UseRouting();
// auth
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    "default",
    "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();