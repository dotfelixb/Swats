using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Keis.Data.Repository;
using Keis.Infrastructure;
using Keis.Model;
using Keis.Model.Domain;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<ConnectionStringOptions>(builder.Configuration.GetSection("ConnectionStrings"));
builder.Services.AddControllersWithViews(o => o.Filters.Add<ValidationFilter>()).AddJsonOptions(o =>
{
    o.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
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
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();
builder.Services.ConfigureApplicationCookie(o =>
{
    o.LoginPath = new PathString("/auth/login");
    o.ExpireTimeSpan = TimeSpan.FromHours(3);
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
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
//app.UseKeisSeed();

app.MapAreaControllerRoute(
    "areadefault",
    "admin",
    "admin/{controller=Settings}/{action=Index}/{id?}");
app.MapControllerRoute(
    "default",
    "{controller=Dashboard}/{action=Index}/{id?}");

app.Run();