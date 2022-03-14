using Htmx;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swats.Model.Commands;

namespace Swats.Controllers.FrontEnd;

public class TicketController : FrontEndController
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Create()
    {
        if (!ModelState.IsValid)
        {
            return Request.IsHtmx()
              ? PartialView("~/Views/Ticket/_CreatePartial.cshtml")
              : View();
        }

        return Request.IsHtmx()
            ? PartialView("~/Views/Ticket/_CreatePartial.cshtml")
            : View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(CreateTicketCommand command)
    {
        if (!ModelState.IsValid)
        {
            return Request.IsHtmx()
              ? PartialView("~/Views/Ticket/_CreatePartial.cshtml", command)
              : View(command);
        }

        return Request.IsHtmx()
            ? PartialView("~/Views/Ticket/_CreatePartial.cshtml", command)
            : View(command);
    }
}
