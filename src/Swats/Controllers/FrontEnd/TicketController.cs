using Htmx;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Swats.Extensions;
using Swats.Model.Commands;
using System.Security.Claims;
using System.Text.Json;

namespace Swats.Controllers.FrontEnd;

public class TicketController : FrontEndController
{
    private readonly ILogger<TicketController> _logger;
    private readonly IMediator _mediatr;

    public TicketController(IHttpContextAccessor httpAccessor
        , ILogger<TicketController> logger
        , IMediator mediatr):base(httpAccessor)
    {
        _logger = logger;
        _mediatr = mediatr;
    }

    #region GET

    public async Task<IActionResult> Index()
    {
        _logger.LogInformation($"{Request.Method}::{nameof(TicketController)}::{nameof(Index)}");

        var query = new ListTicketCommand {Id = UserId , IncludeDepartment = true };
        var result = await _mediatr.Send(query);
        if (result.IsFailed) return NotFound(result.Reasons.FirstOrDefault()?.Message);

        return Request.IsHtmx()
            ? PartialView("~/Views/Ticket/_Index.cshtml", result.Value)
            : View(result.Value);
    }

    public async Task<IActionResult> Edit(string id)
    {
        _logger.LogInformation($"{Request.Method}::{nameof(TicketController)}::{nameof(Edit)}");

        var query = new GetTicketCommand { Id = id };
        var result = await _mediatr.Send(query);

        if (result.IsFailed) return NotFound(result.Reasons.FirstOrDefault()?.Message);
        result.Value.ImageCode = $"{Request.Scheme}://{Request.Host}/ticket/edit/{id}".GenerateQrCode();

        return Request.IsHtmx()
            ? PartialView("~/Views/Ticket/_Edit.cshtml", result.Value)
            : View(result.Value);
    }

    public async Task<IActionResult> Create()
    {
        _logger.LogInformation($"{Request.Method}::{nameof(TicketController)}::{nameof(Create)}");

        var requesterList = await _mediatr.Send(new ListAgentCommand());
        if (requesterList.IsFailed) return BadRequest(requesterList.Reasons.FirstOrDefault()?.Message);

        var ticketTypeList = await _mediatr.Send(new ListTicketTypeCommand());
        if (ticketTypeList.IsFailed) return BadRequest(ticketTypeList.Reasons.FirstOrDefault()?.Message);

        var departmentList = await _mediatr.Send(new ListDepartmentCommand());
        if (departmentList.IsFailed) return BadRequest(departmentList.Reasons.FirstOrDefault()?.Message);

        var teamList = await _mediatr.Send(new ListTeamsCommand());
        if (teamList.IsFailed) return BadRequest(departmentList.Reasons.FirstOrDefault()?.Message);

        var helptopicList = await _mediatr.Send(new ListHelpTopicsCommand());
        if (helptopicList.IsFailed) return BadRequest(helptopicList.Reasons.FirstOrDefault()?.Message);

        CreateTicketCommand command = new()
        {
            AssigneeList = requesterList.Value.Select(s => new SelectListItem
            { Text = $"{s.FirstName} {s.LastName}", Value = s.Id.ToString() }),
            RequesterList = requesterList.Value.Select(s => new SelectListItem
            { Text = $"{s.FirstName} {s.LastName}", Value = s.Id.ToString() }),
            DepartmentList = departmentList.Value.Select(s => new SelectListItem { Text = s.Name, Value = s.Id.ToString() }),
            TeamList = teamList.Value.Select(s => new SelectListItem { Text = s.Name, Value = s.Id.ToString() }),
            TypeList = ticketTypeList.Value.Select(s => new SelectListItem { Text = s.Name, Value = s.Id.ToString() }),
            HelpTopicList =
                helptopicList.Value.Select(s => new SelectListItem { Text = s.Topic, Value = s.Id.ToString() })
        };

        return Request.IsHtmx()
            ? PartialView("~/Views/Ticket/_Create.cshtml", command)
            : View(command);
    }

    #endregion GET

    #region POST

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateTicketCommand command)
    {
        var msg = $"{Request.Method}::{nameof(TicketController)}::{nameof(Create)}";
        _logger.LogInformation(msg);

        if (!ModelState.IsValid)
        {
            _logger.LogError($"{msg} - Invalid model state");
            TempData["CreateError"] = "You have some errors on the form";

            return Request.IsHtmx()
                ? PartialView("~/Views/Ticket/_Create.cshtml", command)
                : View(command);
        }

        command.CreatedBy = UserId;
        var result = await _mediatr.Send(command);
        if (result.IsFailed)
        {
            var reason = result.Reasons.FirstOrDefault()?.Message ?? "CreateError";
            _logger.LogError($"{msg} - {reason}");
            TempData["CreateError"] = reason;

            return Request.IsHtmx()
                ? PartialView("~/Views/Ticket/_Create.cshtml", command)
                : View(command);
        }

        return RedirectToAction("Edit", new { Id = result.Value });
    }

    #endregion POST
}