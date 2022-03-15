using Htmx;
using Microsoft.AspNetCore.Mvc;
using Swats.Controllers;

namespace Swats.Areas.Admin.Controllers;

[Area("admin")]
public class BusinessHourController : FrontEndController
{
    public IActionResult Index()
    {
        return Request.IsHtmx()
                ? PartialView("~/Areas/Admin/Views/BusinessHour/_Index.cshtml")
                : View();
    }

    public IActionResult Create()
    {
        return Request.IsHtmx()
             ? PartialView("~/Areas/Admin/Views/BusinessHour/_Create.cshtml")
             : View();
    }
}