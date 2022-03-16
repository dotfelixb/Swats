using Htmx;
using Microsoft.AspNetCore.Mvc;
using Swats.Controllers;
using Swats.Model.Commands;
using Swats.Model.ViewModel;

namespace Swats.Areas.Admin.Controllers;

[Area("admin")]
public class BusinessHourController : FrontEndController
{
    public IActionResult Index()
    {
        var partial = new IndexPartial
        {
            CreateLocation = "/admin/businesshour/create",
            CreateTitle = "New Hour",
            Title = "Business Hours"
        };

        return Request.IsHtmx()
                ? PartialView("~/Areas/Admin/Views/_Index.cshtml", partial)
                : View(partial);
    }

    public IActionResult Create()
    {
        return Request.IsHtmx()
             ? PartialView("~/Areas/Admin/Views/BusinessHour/_Create.cshtml")
             : View();
    }

    [HttpPost]
    public IActionResult Create(CreateBusinessHourCommand command)
    {
        if (!ModelState.IsValid)
        {
            return Request.IsHtmx()
                ? PartialView("~/Areas/Admin/Views/BusinessHour/_Create.cshtml", command)
                : View(command);
        }

        return Request.IsHtmx()
             ? PartialView("~/Areas/Admin/Views/BusinessHour/_Create.cshtml")
             : View();
    }
}