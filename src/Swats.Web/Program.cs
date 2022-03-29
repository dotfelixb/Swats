using FluentValidation.AspNetCore;
using MediatR;
using Swats.Data.Repository;
using Swats.Infrastructure;
using Swats.Model;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<ConnectionStringOptions>(builder.Configuration.GetSection("ConnectionStrings"));
builder.Services.AddControllersWithViews(o => o.Filters.Add<ValidationFilter>()).AddJsonOptions(o =>
{
    o.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
}).AddFluentValidation(f =>
    f.RegisterValidatorsFromAssemblyContaining<ISwatsInfrastructure>(includeInternalTypes: true));
builder.Services.AddMediatR(typeof(ISwatsInfrastructure));
builder.Services.AddAutoMapper(typeof(ModelProfiles));
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

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
app.UseStaticFiles();
app.UseRouting();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html"); ;

app.Run();
