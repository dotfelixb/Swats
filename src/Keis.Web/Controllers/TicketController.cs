using Keis.Infrastructure.Features.Tickets.AssignTicket;
using Keis.Infrastructure.Features.Tickets.CreateTicket;
using Keis.Infrastructure.Features.Tickets.GetTicket;
using Keis.Infrastructure.Features.Tickets.ListTicket;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Keis.Model;
using Keis.Model.Commands;

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

    [HttpGet("tickets.list", Name = nameof(ListTickets))]
    public async Task<IActionResult> ListTickets([FromQuery] ListTicketCommand command)
    {
        logger.LogInformation($"{Request.Method}::{nameof(TicketController)}::{nameof(ListTickets)}");

        var result = await mediatr.Send(command);
        if (result.IsFailed)
        {
            return NotFound(new ErrorResult { Ok = false });
        }

        return Ok(result.Value);
    }

    [HttpGet("tickets.get", Name =nameof(GetTicket))]
    public async Task<IActionResult> GetTicket([FromQuery] GetTicketCommand command)
    {
        logger.LogInformation($"{Request.Method}::{nameof(TicketController)}::{nameof(GetTicket)}");

        var result = await mediatr.Send(command);
        if (result.IsFailed)
        {
            var error = result.Reasons.FirstOrDefault()?.Message;
            return NotFound(new ErrorResult { Ok = false, Errors = new []{error }});
        }

        return Ok(result.Value);
    }

    [HttpPost("tickets.create", Name = nameof(CreateTicket))]
    public async Task<IActionResult> CreateTicket( CreateTicketCommand command)
    {
        logger.LogInformation($"{Request.Method}::{nameof(TicketController)}::{nameof(CreateTicket)}");

        var result = await mediatr.Send(command);
        if (result.IsFailed)
        {
            var error = result.Reasons.FirstOrDefault()?.Message;
            return BadRequest(new ErrorResult { Ok = false, Errors = new[] { error } });
        }

        var baseUri = $"{Request.Scheme}://{Request.Host}";
        var uri = $"{baseUri}/methods/tickets.get?id={result.Value}";

        return Created(uri, result.Value);
    }

    [HttpPatch("tickets.assign", Name = nameof(AssignTicket))]
    public Task<IActionResult> AssignTicket(AssignTicketCommand command)
    {
        throw new NotImplementedException();
    }

    [HttpPatch("tickets.department", Name = nameof(AssignDepartmentTicket))]
    public Task<IActionResult> AssignDepartmentTicket(AssignTicketDepartmentCommand command)
    {
        throw new NotImplementedException();
    }

    [HttpPatch("tickets.team", Name = nameof(AssignTeamTicket))]
    public Task<IActionResult> AssignTeamTicket(AssignTicketTeamCommand command)
    {
        throw new NotImplementedException();
    }
}