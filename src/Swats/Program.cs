using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Swats.Data.Repository;
using Swats.Infrastructure;
using Swats.Model;
using Swats.Model.Domain;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<ConnectionStringOptions>(builder.Configuration.GetSection("ConnectionStrings"));
builder.Services.AddControllersWithViews(o => o.Filters.Add<ValidationFilter>())
    .AddFluentValidation(f => f.RegisterValidatorsFromAssemblyContaining<ISwatsInfrastructure>(includeInternalTypes: true));
builder.Services.AddMediatR(typeof(ISwatsInfrastructure));
builder.Services.AddAutoMapper(typeof(ModelProfiles));

// auth
builder.Services.AddIdentity<AuthUser, AuthRole>(o =>
{
    o.Password.RequireDigit = false;
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
builder.Services.AddSingleton<ITicketRepository, TicketRepository>();

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
//app.UseSwatsSeed();

app.MapAreaControllerRoute(
    name: "areadefault",
    areaName: "admin",
    pattern: "admin/{controller=Settings}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}");

app.Run();