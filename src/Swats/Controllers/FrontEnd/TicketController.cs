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
        return View();
    }

    public IActionResult CreatePartial()
    {
        return PartialView("_CreatePartial");
    }
}
