using Htmx;
using Microsoft.AspNetCore.Mvc;

namespace Swats.Controllers.FrontEnd;

public class DepartmentController : FrontEndController {

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
