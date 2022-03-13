using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Swats.Model.Domain;

namespace Swats.Infrastructure;

public class SwatsSeedMiddleware
{
    private readonly RequestDelegate _next;
    private readonly UserManager<AuthUser> _userManager;
    private readonly SignInManager<AuthUser> _signInManager;

    public SwatsSeedMiddleware(RequestDelegate next
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