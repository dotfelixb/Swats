using Htmx;
using Microsoft.AspNetCore.Mvc;
using Swats.Controllers;

namespace Swats.Areas.Admin.Controllers;

[Area("admin")]
public class HelpTopicController : FrontEndController
{
    public IActionResult Index()
    {
        return Request.IsHtmx()
                ? PartialView("~/Areas/Admin/Views/HelpTopic/_Index.cshtml")
                : View();
    }

    public IActionResult Create()
    {
        return Request.IsHtmx()
             ? PartialView("~/Areas/Admin/Views/HelpTopic/_Create.cshtml")
             : View();
    }
}
