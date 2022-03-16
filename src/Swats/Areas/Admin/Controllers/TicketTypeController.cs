using Htmx;
using Microsoft.AspNetCore.Mvc;
using Swats.Controllers;
using Swats.Model.Commands;
using Swats.Model.ViewModel;

namespace Swats.Areas.Admin.Controllers;

[Area("admin")]
public class TicketTypeController : FrontEndController
{
    public IActionResult Index()
    {
        var partial = new IndexPartial
        {
            CreateLocation = "/admin/tickettype/create",
            CreateTitle = "New Ticket Type",
            Title = "Ticket Types"
        };

        return Request.IsHtmx()
                ? PartialView("~/Areas/Admin/Views/_Index.cshtml", partial)
                : View(partial);
    }

    public IActionResult Create()
    {
        return Request.IsHtmx()
             ? PartialView("~/Areas/Admin/Views/TicketType/_Create.cshtml")
             : View();
    }

    [HttpPost]

    public IActionResult Create(CreateTicketTypeCommand command)
    {
        if (!ModelState.IsValid)
        {
            return Request.IsHtmx()
                ? PartialView("~/Areas/Admin/Views/TicketType/_Create.cshtml", command)
                : View(command);
        }

        ViewData["EditTitle"] = "<put new type id here>";
        return RedirectToAction("Edit");
    }
}
