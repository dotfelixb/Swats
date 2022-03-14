using Microsoft.AspNetCore.Mvc;

namespace Swats.Controllers.FrontEnd;

public class SettingsController : FrontEndController
{
    public IActionResult Index()
    {
        return View();
    }
}