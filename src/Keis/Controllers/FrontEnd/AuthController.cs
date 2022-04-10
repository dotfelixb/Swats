using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Keis.Extensions;
using Keis.Infrastructure.Features.Users.Register;
using Keis.Model.Commands;
using Keis.Model.Domain;
using System.Security.Claims;

namespace Keis.Controllers.FrontEnd;

public class AuthController : FrontEndController
{
    private readonly SignInManager<AuthUser> _signInManager;
    private readonly UserManager<AuthUser> _userManager;
    private readonly ILogger<AuthController> _logger;
    private readonly IMediator _mediatr;

    public AuthController(IHttpContextAccessor httpAccessor
        , UserManager<AuthUser> userManager
        , SignInManager<AuthUser> signInManager
        , ILogger<AuthController> logger
        , IMediator mediatr): base(httpAccessor)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
        _mediatr = mediatr;
    }

    public IActionResult Index()
    {
        return Content("Not sure why you are here 🤷🏾‍♀️");
    }

    public async Task<IActionResult> Logout()
    {
        _logger.LogInformation($"{Request.Method}::{nameof(AuthController)}::{nameof(Logout)}");

        await _signInManager.SignOutAsync();
        return LocalRedirect("/auth/login");
    }

    [AllowAnonymous]
    public async Task<IActionResult> Login(string returnUrl = null)
    {
        // Clear the existing external cookie to ensure a clean login process
        await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [AllowAnonymous]
    public IActionResult Register()
    {
        return Content("Ask your admin to register you");
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginCommand command, string returnUrl = null)
    {
        var msg = $"{Request.Method}::{nameof(AuthController)}::{nameof(Login)}";
        _logger.LogInformation(msg);

        ViewData["ReturnUrl"] = returnUrl;
        if (!ModelState.IsValid)
        {
            _logger.LogError($"{msg} - Invalid model state");
            TempData["LoginStatus"] = "Form Errors";

            return View(command);
        }

        var result =
            await _signInManager.PasswordSignInAsync(command.UserName, command.Password, command.RememberMe, false);
        if (result.Succeeded)
        {
            var user = await _userManager.FindByNameAsync(command.UserName);
            if (user == null)
            {
                var unknown = $"Unknown User [{command.UserName}]";

                _logger.LogError($"{msg} - {unknown}");
                ModelState.AddModelError(string.Empty, unknown);

                return View(command);
            }

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id),
                new(ClaimTypes.Name, user.UserName),
                new(ClaimTypes.Email, user.Email)
            };

            var claimPrincipal =
                new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = command.RememberMe
            };

            // log user login - we need to wait
            var ipAddress = Request.HttpContext.GetClientAddress();
            var loginLog = new LoginLogCommand
            {
                AuthUser = user.Id,
                Address = ipAddress
            };
            _ = await _mediatr.Send(loginLog);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimPrincipal,
                authProperties);

            _logger.LogInformation($"{msg} - Login success for user [{command.UserName}]");
            return LocalRedirect(returnUrl ?? "/");
        }

        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        TempData["LoginStatus"] = "Login Failed!";

        _logger.LogError($"{msg} - Login failed for user [{command.UserName}]");
        return View(command);
    }

    [HttpPost]
    [AllowAnonymous]
    public IActionResult Register(RegisterCommand command)
    {
        return View();
    }
}