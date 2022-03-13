using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swats.Infrastructure.Features.Users.Login;
using Swats.Infrastructure.Features.Users.Register;

namespace Swats.Controllers.FrontEnd;

public class UserController : FrontEndController
{
    public IActionResult Index()
    {
        return View();
    }

    [AllowAnonymous]
    public IActionResult Login()
    {
        return View();
    }

    [AllowAnonymous]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    public IActionResult Login(LoginCommand command)
    {
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    public IActionResult Register(RegisterCommand command)
    {
        return View();
    }
}
