using Microsoft.AspNetCore.Mvc;

namespace Swats.Controllers.FrontEnd;

public class UserController : FrontEndController
{
    public IActionResult Index()
    {
        return View();
    }
}