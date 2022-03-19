using Htmx;
using Microsoft.AspNetCore.Mvc;
using Swats.Controllers;
using Swats.Model.ViewModel;

namespace Swats.Areas.Admin.Controllers;

[Area("admin")]
public class HelpTopicController : FrontEndController
{
    public IActionResult Index()
    {
        var partial = new IndexPartial
        {
            CreateLocation = "/admin/helptopic/create",
            CreateTitle = "New Topic",
            Title = "Help Topics"
        };

        return Request.IsHtmx()
                ? PartialView("~/Areas/Admin/Views/_Index.cshtml", partial)
                : View(partial);
    }

    public IActionResult Create()
    {
        return Request.IsHtmx()
             ? PartialView("~/Areas/Admin/Views/HelpTopic/_Create.cshtml")
             : View();
    }
}
