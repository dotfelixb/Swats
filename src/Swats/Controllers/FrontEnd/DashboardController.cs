using Microsoft.AspNetCore.Mvc;
using Swats.Model.ViewModel;

namespace Swats.Controllers.FrontEnd;

public class DashboardController : FrontEndController
{
    public IActionResult Index()
    {
        return View();
    }
}
