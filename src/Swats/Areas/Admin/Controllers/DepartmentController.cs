using Htmx;
using Microsoft.AspNetCore.Mvc;
using Swats.Controllers;
using Swats.Model.ViewModel;

namespace Swats.Areas.Admin.Controllers;

[Area("admin")]
public class DepartmentController : FrontEndController
{
    public IActionResult Index()
    {
        var partial = new IndexPartial
        {
            CreateLocation = "/admin/department/create",
            CreateTitle = "New Department",
            Title = "Departments"

        };
        return Request.IsHtmx()
             ? PartialView("~/Areas/Admin/Views/_Index.cshtml",partial)
             : View(partial);
    }

    public IActionResult Create()
    {
        return Request.IsHtmx()
             ? PartialView("~/Areas/Admin/Views/Department/_Create.cshtml")
             : View();
    }
}
