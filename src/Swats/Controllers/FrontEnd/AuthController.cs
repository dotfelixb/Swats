using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swats.Infrastructure.Features.Users.Register;
using Swats.Model.Commands;
using Swats.Model.Domain;
using System.Security.Claims;

namespace Swats.Controllers.FrontEnd;

public class AuthController : FrontEndController
{
    private readonly UserManager<AuthUser> _userManager;
    private readonly SignInManager<AuthUser> _signInManager;

    public AuthController(UserManager<AuthUser> userManager
        , SignInManager<AuthUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public IActionResult Index()
    {
        return Content("Not sure why you are here 🤷🏾‍♀️");
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
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginCommand command, string returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        if (!ModelState.IsValid)
        {
            TempData["LoginStatus"] = "Form Errors";
            return View(command);
        }

        var result = await _signInManager.PasswordSignInAsync(command.UserName, command.Password, command.RememberMe, lockoutOnFailure: false);
        if (result.Succeeded)
        {
            var user = await _userManager.FindByNameAsync(command.UserName);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, $"Unknown User [{command.UserName}]");
                return View(command);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var claimPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = command.RememberMe
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimPrincipal, authProperties);
            return LocalRedirect(returnUrl);
        }

        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        TempData["LoginStatus"] = "Login Failed!";
        return View(command);
    }

    [HttpPost]
    [AllowAnonymous]
    public IActionResult Register(RegisterCommand command)
    {
        return View();
    }
}