using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swats.Infrastructure.Features.Users.Register;
using Swats.Model.Commands;
using Swats.Model.Domain;

namespace Swats.Controllers.FrontEnd;

public class UserController : FrontEndController
{
    private readonly UserManager<AuthUser> _userManager;
    private readonly SignInManager<AuthUser> _signInManager;

    public UserController(UserManager<AuthUser> userManager
        , SignInManager<AuthUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public IActionResult Index()
    {
        return View();
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
    public async Task<IActionResult> Login(LoginCommand command, string returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;

        var result = await _signInManager.PasswordSignInAsync(command.UserName, command.Password, command.RememberMe, lockoutOnFailure: false);
        if (result.Succeeded)
        {
            return LocalRedirect(returnUrl);
        }

        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        return View(command);
    }

    [HttpPost]
    [AllowAnonymous]
    public IActionResult Register(RegisterCommand command)
    {
        return View();
    }

    // TODO : Remove
    //[AllowAnonymous]
    //public async Task<IActionResult> Force()
    //{
    //    var user = new AuthUser
    //    {
    //        Id = NewId.NextGuid(),
    //        UserName = "system",
    //        Email = "system@swats.app",
    //        NormalizedUserName = "SYSTEM",
    //        RowVersion = Guid.NewGuid(),
    //    };

    //    var result = await _userManager.CreateAsync(user, "root@pAss");
    //    if (!result.Succeeded)
    //    {
    //        return Content("failed!");
    //    }

    //    return Content("success!");
    //}
}