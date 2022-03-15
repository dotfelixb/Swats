using Htmx;
using Microsoft.AspNetCore.Mvc;
using Swats.Model.Commands;

namespace Swats.Controllers.FrontEnd;

public class TicketController : FrontEndController
{
    public IActionResult Index()
    {
        return Request.IsHtmx()
            ? PartialView("~/Views/Ticket/_Index.cshtml")
            : View();
    }

    public IActionResult Create()
    {
        return Request.IsHtmx()
            ? PartialView("~/Views/Ticket/_Create.cshtml")
            : View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(CreateTicketCommand command)
    {
        if (!ModelState.IsValid)
        {
            return Request.IsHtmx()
              ? PartialView("~/Views/Ticket/_Create.cshtml", command)
              : View(command);
        }

        return RedirectToAction(actionName: "Index");
    }
}
