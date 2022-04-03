using System.Net.Mime;
using System.Text;
using FluentValidation.AspNetCore;
using MediatR;
using Swats.Data.Repository;
using Swats.Infrastructure;
using Swats.Model;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Swats.Model.Domain;

var builder = WebApplication.CreateBuilder(args);
var bearerKey = builder.Configuration.GetSection("SecurityKey:Bearer").Value;

// Add services to the container.
builder.Services.Configure<ConnectionStringOptions>(builder.Configuration.GetSection("ConnectionStrings"));
builder.Services.Configure<SecurityKeyOptions>(builder.Configuration.GetSection("SecurityKey"));
builder.Services.AddControllersWithViews(o => o.Filters.Add<ValidationFilter>()).AddJsonOptions(o =>
{
    //o.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
}).AddFluentValidation(f =>
    f.RegisterValidatorsFromAssemblyContaining<ISwatsInfrastructure>(includeInternalTypes: true));
builder.Services.AddMediatR(typeof(ISwatsInfrastructure));
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStatusCodePages(async statusCodeContext =>
{
    // using static System.Net.Mime.MediaTypeNames;
    statusCodeContext.HttpContext.Response.ContentType = MediaTypeNames.Application.Json;

    await statusCodeContext.HttpContext.Response.WriteAsync(
        $"{statusCodeContext.HttpContext.Response.StatusCode}");
});
app.UseStaticFiles();
app.UseRouting();
// auth
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html"); 

app.Run();
