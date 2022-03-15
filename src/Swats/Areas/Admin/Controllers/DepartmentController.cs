using Htmx;
using Microsoft.AspNetCore.Mvc;
using Swats.Controllers;

namespace Swats.Areas.Admin.Controllers;

[Area("admin")]
public class DepartmentController : FrontEndController
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Create()
    {
        return Request.IsHtmx()
             ? PartialView("~/Views/Department/_CreatePartial.cshtml")
             : View();
    }
}