using Microsoft.AspNetCore.Mvc;

namespace Swats.Controllers.FrontEnd;

public class DashboardController : FrontEndController
{
    public IActionResult Index()
    {
        return View();
    }
}