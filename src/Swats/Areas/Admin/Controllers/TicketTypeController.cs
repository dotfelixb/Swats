using Htmx;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swats.Controllers;
using Swats.Extensions;
using Swats.Model.Commands;
using System.Security.Claims;

namespace Swats.Areas.Admin.Controllers;

[Area("admin")]
public class TicketTypeController : FrontEndController
{
    private readonly ILogger<TicketTypeController> _logger;
    private readonly IMediator _mediatr;

    public TicketTypeController(IHttpContextAccessor httpAccessor
        , ILogger<TicketTypeController> logger
        , IMediator mediatr) : base(httpAccessor)
    {
        _logger = logger;
        _mediatr = mediatr;
    }

    #region POST

    [HttpPost]
    public async Task<IActionResult> Create(CreateTicketTypeCommand command)
    {
        var msg = $"{Request.Method}::{nameof(TicketTypeController)}::{nameof(Create)}";
        _logger.LogInformation(msg);

        if (!ModelState.IsValid)
            return Request.IsHtmx()
                ? PartialView("~/Areas/Admin/Views/TicketType/_Create.cshtml", command)
                : View(command);

        command.CreatedBy = UserId;
        var result = await _mediatr.Send(command);
        if (result.IsFailed)
        {
            var reason = result.Reasons.FirstOrDefault()?.Message ?? "CreateError";
            _logger.LogError($"{msg} - {reason}");
            TempData["CreateError"] = reason;

            return Request.IsHtmx()
                ? PartialView("~/Areas/Admin/Views/TicketType/_Create.cshtml", command)
                : View(command);
        }

        return RedirectToAction("Edit", new { Id = result.Value });
    }

    #endregion POST

    #region GET

    public async Task<IActionResult> Index()
    {
        _logger.LogInformation($"{Request.Method}::{nameof(TicketTypeController)}::{nameof(Index)}");
        var query = new ListTicketTypeCommand();
        var result = await _mediatr.Send(query);
        if (result.IsFailed) return NotFound(result.Reasons.FirstOrDefault()?.Message);

        return Request.IsHtmx()
            ? PartialView("~/Areas/Admin/Views/TicketType/_Index.cshtml", result.Value)
            : View(result.Value);
    }

    public async Task<IActionResult> Edit(string id)
    {
        var query = new GetTicketTypeCommand { Id = id };
        var result = await _mediatr.Send(query);

        if (result.IsFailed) return NotFound(result.Reasons.FirstOrDefault()?.Message);
        result.Value.ImageCode = $"{Request.Scheme}://{Request.Host}/admin/tickettype/edit/{id}".GenerateQrCode();

        return Request.IsHtmx()
            ? PartialView("~/Areas/Admin/Views/TicketType/_Edit.cshtml", result.Value)
            : View(result.Value);
    }

    public IActionResult Create()
    {
        _logger.LogInformation($"{Request.Method}::{nameof(TicketTypeController)}::{nameof(Create)}");

        return Request.IsHtmx()
            ? PartialView("~/Areas/Admin/Views/TicketType/_Create.cshtml")
            : View();
    }

    #endregion GET
}