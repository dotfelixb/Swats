using Htmx;
using Microsoft.AspNetCore.Mvc;
using Swats.Controllers;

namespace Swats.Areas.Admin.Controllers;

[Area("admin")]
public class SettingsController : FrontEndController
{
    public IActionResult Index()
    {
        return Request.IsHtmx()
            ? PartialView("~/Areas/Admin/Views/Settings/_Index.cshtml")
            : View();
    }
}
