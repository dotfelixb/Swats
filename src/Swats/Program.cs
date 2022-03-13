using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Swats.Data.Repository;
using Swats.Model;
using Swats.Model.Domain;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<ConnectionStringOptions>(builder.Configuration.GetSection("ConnectionStrings"));
builder.Services.AddControllersWithViews();

// auth
builder.Services
    .AddIdentity<AuthUser, AuthRole>()
    .AddDefaultTokenProviders();
builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(o => o.LoginPath = new PathString("user/login"));
builder.Services
    .AddAuthorization(o => o.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build());
builder.Services.AddTransient<IUserStore<AuthUser>, AuthUserRepository>();
builder.Services.AddTransient<IRoleStore<AuthRole>, AuthRoleRepository>();

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
app.UseAuthorization();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}");

app.Run();