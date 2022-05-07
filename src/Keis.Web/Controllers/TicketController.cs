using Keis.Infrastructure.Features.Tickets.AssignTicket;
using Keis.Infrastructure.Features.Tickets.CreateTicket;
using Keis.Infrastructure.Features.Tickets.GetTicket;
using Keis.Infrastructure.Features.Tickets.ListTicket;
using Keis.Model;
using Keis.Model.Commands;
using Keis.Model.Queries;
using Keis.Web.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Keis.Web.Controllers;

public class TicketController : MethodController
{
    private readonly ILogger<TicketController> logger;
    private readonly IMediator mediatr;

    public TicketController(ILogger<TicketController> logger, IMediator mediatr)
    {
        this.logger = logger;
        this.mediatr = mediatr;
    }

    [HttpGet("ticket.list", Name = nameof(ListTickets))]
    public async Task<IActionResult> ListTickets([FromQuery] ListTicketCommand command)
    {
        const string msg = $"GET::{nameof(TicketController)}::{nameof(ListTickets)}";
        logger.LogInformation(msg);

        var result = await mediatr.Send(command);
        if (result.IsFailed)
            return NotFound(new ErrorResult {
                Ok = false,
                Errors = result.Reasons.Select(s => s.Message)
            });

        return Ok(new ListResult<FetchTicket>
        {
            Ok = true,
            Data = result.Value
        });
    }

    [HttpGet("ticket.get", Name = nameof(GetTicket))]
    public async Task<IActionResult> GetTicket([FromQuery] GetTicketCommand command)
    {
        const string msg = $"GET::{nameof(TicketController)}::{nameof(GetTicket)}";
        logger.LogInformation(msg);

        var result = await mediatr.Send(command);
        if (result.IsFailed)
            return BadRequest(new ErrorResult
            {
                Ok = false,
                Errors = result.Reasons.Select(s => s.Message)
            });

        return Ok(new SingleResult<FetchTicket>
        {
            Ok = true,
            Data = result.Value
        });
    }

    [HttpPost("ticket.create", Name = nameof(CreateTicket))]
    public async Task<IActionResult> CreateTicket(CreateTicketCommand command)
    {
        const string msg = $"POST::{nameof(TicketController)}::{nameof(CreateTicket)}";
        logger.LogInformation(msg);

        command.CreatedBy = Request.HttpContext.UserId();
        var result = await mediatr.Send(command);

        if (result.IsFailed)
            return BadRequest(new ErrorResult
            {
                Ok = false,
                Errors = result.Reasons.Select(s => s.Message)
            });

        var uri = $"/methods/ticket.get?id={result.Value}";
        return Created(uri, new SingleResult<string>
        {
            Ok = true,
            Data = result.Value
        });
    }

    [HttpPatch("ticket.assign", Name = nameof(AssignTicket))]
    public Task<IActionResult> AssignTicket(AssignTicketCommand command)
    {
        throw new NotImplementedException();
    }

    [HttpPatch("ticket.department", Name = nameof(AssignDepartmentTicket))]
    public Task<IActionResult> AssignDepartmentTicket(AssignTicketDepartmentCommand command)
    {
        throw new NotImplementedException();
    }

    [HttpPatch("ticket.team", Name = nameof(AssignTeamTicket))]
    public Task<IActionResult> AssignTeamTicket(AssignTicketTeamCommand command)
    {
        throw new NotImplementedException();
    }
}