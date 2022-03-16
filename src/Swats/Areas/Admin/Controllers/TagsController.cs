using Htmx;
using Microsoft.AspNetCore.Mvc;
using Swats.Controllers;
using Swats.Model.ViewModel;

namespace Swats.Areas.Admin.Controllers;

[Area("admin")]
public class TagsController : FrontEndController
{
    public IActionResult Index()
    {
        var partial = new IndexPartial
        {
            CreateLocation = "/admin/tags/create",
            CreateTitle = "New Tag",
            Title = "Tags"
        };

        return Request.IsHtmx()
                ? PartialView("~/Areas/Admin/Views/_Index.cshtml", partial)
                : View(partial);
    }

    public IActionResult Create()
    {
        return Request.IsHtmx()
             ? PartialView("~/Areas/Admin/Views/Tags/_Create.cshtml")
             : View();
    }
}
