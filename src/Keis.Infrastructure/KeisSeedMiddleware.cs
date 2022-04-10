using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Keis.Model.Domain;

namespace Keis.Infrastructure;

public class KeisSeedMiddleware
{
    private readonly RequestDelegate _next;
    private readonly SignInManager<AuthUser> _signInManager;
    private readonly UserManager<AuthUser> _userManager;

    public KeisSeedMiddleware(RequestDelegate next
        , UserManager<AuthUser> userManager
        , SignInManager<AuthUser> signInManager)
    {
        _next = next;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public Task Invoke(HttpContext context)
    {
        return _next(context);
    }
}