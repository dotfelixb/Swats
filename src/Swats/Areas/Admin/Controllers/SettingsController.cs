using Htmx;
using Microsoft.AspNetCore.Mvc;
using Swats.Controllers;
using Swats.Model.ViewModel;

namespace Swats.Areas.Admin.Controllers;

[Area("admin")]
public class SettingsController : FrontEndController
{
    public IActionResult Index()
    {
        return  View();
    }
}
