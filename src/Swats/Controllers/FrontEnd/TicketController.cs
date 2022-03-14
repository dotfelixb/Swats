using Htmx;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Swats.Controllers.FrontEnd;

public class TicketController : FrontEndController
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Create()
    {
        return Request.IsHtmx()
            ? PartialView("~/Views/Ticket/_CreatePartial.cshtml")
            : View();
    }
}
