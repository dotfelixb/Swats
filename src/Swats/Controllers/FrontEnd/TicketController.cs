using Htmx;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Swats.Model.Commands;
using Swats.Model.Domain;
using Swats.Model.Queries;

namespace Swats.Controllers.FrontEnd;

public class TicketController : FrontEndController
{
    private readonly IHttpContextAccessor _httpAccessor;
    private readonly ILogger<TicketController> _logger;
    private readonly IMediator _mediatr;

    public TicketController(IHttpContextAccessor httpAccessor
        , ILogger<TicketController> logger
        , IMediator mediatr)
    {
        _httpAccessor = httpAccessor;
        _logger = logger;
        _mediatr = mediatr;
    }


    public IActionResult Index()
    {
        _logger.LogInformation($"{Request.Method}::{nameof(TicketController)}::{nameof(Index)}");
        return Request.IsHtmx()
            ? PartialView("~/Views/Ticket/_Index.cshtml")
            : View();
    }

    public async Task<IActionResult> Create()
    {
        _logger.LogInformation($"{Request.Method}::{nameof(TicketController)}::{nameof(Index)}");
       
        var ticketTypeResult = await _mediatr.Send(new ListTicketTypeCommand { });
        if (ticketTypeResult.IsFailed)
        {
            return BadRequest(ticketTypeResult.Reasons.FirstOrDefault()?.Message);
        }

        CreateTicketCommand command = new()
        {
            TicketTypes = ticketTypeResult.Value.Select(s => new SelectListItem { Text = s.Name, Value = s.Id.ToString() })
        };

        return Request.IsHtmx()
            ? PartialView("~/Views/Ticket/_Create.cshtml", command)
            : View(command);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(CreateTicketCommand command)
    {
        _logger.LogInformation($"{Request.Method}::{nameof(TicketController)}::{nameof(Index)}");
        if (!ModelState.IsValid)
        {
            return Request.IsHtmx()
              ? PartialView("~/Views/Ticket/_Create.cshtml", command)
              : View(command);
        }

        return RedirectToAction(actionName: "Index");
    }
}
