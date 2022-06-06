using Keis.Infrastructure.Features.Agents.GetAgent;
using Keis.Infrastructure.Features.Department.GetDepartment;
using Keis.Infrastructure.Features.HelpTopic.GetHelpTopic;
using Keis.Infrastructure.Features.Teams.GetTeam;
using Keis.Infrastructure.Features.Tickets.AssignTicket;
using Keis.Infrastructure.Features.Tickets.ChangeDepartment;
using Keis.Infrastructure.Features.Tickets.ChangeDue;
using Keis.Infrastructure.Features.Tickets.ChangePriority;
using Keis.Infrastructure.Features.Tickets.ChangeStatus;
using Keis.Infrastructure.Features.Tickets.ChangeTeam;
using Keis.Infrastructure.Features.Tickets.ChangeTopic;
using Keis.Infrastructure.Features.Tickets.ChangeType;
using Keis.Infrastructure.Features.Tickets.CreateComment;
using Keis.Infrastructure.Features.Tickets.CreateTicket;
using Keis.Infrastructure.Features.Tickets.GetTicket;
using Keis.Infrastructure.Features.Tickets.ListComments;
using Keis.Infrastructure.Features.Tickets.ListTicket;
using Keis.Infrastructure.Features.TicketTypes.GetTicketType;
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

        var commentResult = await mediatr.Send(new ListTicketCommentCommand { TicketId = command.Id });
        if (commentResult.IsSuccess) result.Value.TicketComments = commentResult.Value;

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

        //get requester details
        var agentResult = await mediatr.Send(new GetAgentCommand { Id = command.Requester });
        if (agentResult.IsSuccess)
        {
            command.RequesterEmail = agentResult.Value.Email;
            command.RequesterName = agentResult.Value.Name;
        }

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

    [HttpPost("ticket.postcomment", Name = nameof(CreateTicketComment))]
    public async Task<IActionResult> CreateTicketComment(CreateTicketCommentCommand command)
    {
        const string msg = $"POST::{nameof(TicketController)}::{nameof(CreateTicketComment)}";
        logger.LogInformation(msg);

        var userId = Request.HttpContext.UserId();
        if(userId != null && userId == "00000000-0000-0000-0000-000000000001")
            return Unauthorized(new ErrorResult
            {
                Ok = false,
                Errors = new[] {"User can't post a comment"}
            });

        var agentResult = await mediatr.Send(new GetAgentCommand { Id = userId });
        if (agentResult.IsFailed)
            return BadRequest(new ErrorResult
            {
                Ok = false,
                Errors = agentResult.Reasons.Select(s => s.Message)
            });

        command.FromEmail = agentResult.Value.Email;
        command.FromName = agentResult.Value.Name;
        command.CreatedBy = userId;

        var result = await mediatr.Send(command);
        if (result.IsFailed)
            return BadRequest(new ErrorResult
            {
                Ok = false,
                Errors = result.Reasons.Select(s => s.Message)
            });

        var uri = $"/methods/ticket.get?id={command.TicketId}";
        return Created(uri, new SingleResult<string>
        {
            Ok = true,
            Data = result.Value
        });
    }

    [HttpPatch("ticket.assign", Name = nameof(AssignTicket))]
    public async Task<IActionResult> AssignTicket(AssignTicketCommand command)
    {
        const string msg = $"PATCH::{nameof(TicketController)}::{nameof(AssignTicket)}";
        logger.LogInformation(msg);

        command.CreatedBy = Request.HttpContext.UserId();
        var result = await mediatr.Send(command);

        if (result.IsFailed)
            return BadRequest(new ErrorResult
            {
                Ok = false,
                Errors = result.Reasons.Select(s => s.Message)
            });

        var agentResult = await mediatr.Send(new GetAgentCommand { Id = command.AssignedTo });
        if (agentResult.IsFailed)
            return BadRequest(new ErrorResult
            {
                Ok = false,
                Errors = agentResult.Reasons.Select(s => s.Message)
            });

        return Ok(new SingleResult<object>
        {
            Ok = true,
            Data = new { agentResult.Value.Id, agentResult.Value.Name }
        });
    }

    [HttpPatch("ticket.status", Name = nameof(ChangeStatus))]
    public async Task<IActionResult> ChangeStatus(ChangeStatusCommand command)
    {
        const string msg = $"PATCH::{nameof(TicketController)}::{nameof(ChangeStatus)}";
        logger.LogInformation(msg);

        command.CreatedBy = Request.HttpContext.UserId();
        var result = await mediatr.Send(command);

        if(result.IsFailed)
            return BadRequest(new ErrorResult
            {
                Ok = false,
                Errors = result.Reasons.Select(s => s.Message)
            });

        return Ok(new SingleResult<string>
        {
            Ok = true,
            Data = result.Value
        });
    }

    [HttpPatch("ticket.priority", Name = nameof(ChangePriority))]
    public async Task<IActionResult> ChangePriority(ChangePriorityCommand command)
    {
        const string msg = $"PATCH::{nameof(TicketController)}::{nameof(ChangePriority)}";
        logger.LogInformation(msg);

        command.CreatedBy = Request.HttpContext.UserId();
        var result = await mediatr.Send(command);

        if(result.IsFailed)
            return BadRequest(new ErrorResult
            {
                Ok = false,
                Errors = result.Reasons.Select(s => s.Message)
            });

        return Ok(new SingleResult<string>
        {
            Ok = true,
            Data = result.Value
        });
    }

    [HttpPatch("ticket.department", Name = nameof(ChangeTicketDepartment))]
    public async Task<IActionResult> ChangeTicketDepartment(ChangeDepartmentCommand command)
    {
        const string msg = $"PATCH::{nameof(TicketController)}::{nameof(ChangeTicketDepartment)}";
        logger.LogInformation(msg);

        command.CreatedBy = Request.HttpContext.UserId();
        var result = await mediatr.Send(command);

        if (result.IsFailed)
            return BadRequest(new ErrorResult
            {
                Ok = false,
                Errors = result.Reasons.Select(s => s.Message)
            });

        var departmentResult = await mediatr.Send(new GetDepartmentCommand { Id = command.Department });
        if (departmentResult.IsFailed)
            return NotFound(new ErrorResult
            {
                Ok = false,
                Errors = departmentResult.Reasons.Select(s => s.Message)
            });

        return Ok(new SingleResult<object>
        {
            Ok = true,
            Data = new { departmentResult.Value.Id, departmentResult.Value.Name }
        });
    }

    [HttpPatch("ticket.team", Name = nameof(ChangeTicketTeam))]
    public async Task<IActionResult> ChangeTicketTeam(ChangeTeamCommand command)
    {
        const string msg = $"PATCH::{nameof(TicketController)}::{nameof(ChangeTicketTeam)}";
        logger.LogInformation(msg);

        command.CreatedBy = Request.HttpContext.UserId();
        var result = await mediatr.Send(command);

        if (result.IsFailed)
            return BadRequest(new ErrorResult
            {
                Ok = false,
                Errors = result.Reasons.Select(s => s.Message)
            });

        var teamResult = await mediatr.Send(new GetTeamCommand { Id = command.Team });
        if (teamResult.IsFailed)
            return NotFound(new ErrorResult
            {
                Ok = false,
                Errors = teamResult.Reasons.Select(s => s.Message)
            });

        return Ok(new SingleResult<object>
        {
            Ok = true,
            Data = new { teamResult.Value.Id, teamResult.Value.Name }
        });
    }


    [HttpPatch("ticket.duedate", Name = nameof(ChangeDueDate))]
    public async Task<IActionResult> ChangeDueDate(ChangeDueCommand command)
    {
        const string msg = $"PATCH::{nameof(TicketController)}::{nameof(ChangeDueDate)}";
        logger.LogInformation(msg);

        command.CreatedBy = Request.HttpContext.UserId();
        var result = await mediatr.Send(command);

        if (result.IsFailed)
            return BadRequest(new ErrorResult
            {
                Ok = false,
                Errors = result.Reasons.Select(s => s.Message)
            });

        return Ok(new SingleResult<DateTimeOffset>
        {
            Ok = true,
            Data = result.Value
        });
    }

     [HttpPatch("ticket.tickettype", Name = nameof(ChangeTicketType))]
    public async Task<IActionResult> ChangeTicketType(ChangeTicketTypeCommand command)
    {
        const string msg = $"PATCH::{nameof(TicketController)}::{nameof(ChangeTicketType)}";
        logger.LogInformation(msg);

        command.CreatedBy = Request.HttpContext.UserId();
        var result = await mediatr.Send(command);

        if (result.IsFailed)
            return BadRequest(new ErrorResult
            {
                Ok = false,
                Errors = result.Reasons.Select(s => s.Message)
            });

        var typeResult = await mediatr.Send(new GetTicketTypeCommand{Id = command.TicketType});
        if (typeResult.IsFailed)
            return   NotFound(new ErrorResult
            {
                Ok = false,
                Errors = typeResult.Reasons.Select(s=> s.Message)
            });

        return Ok(new SingleResult<object>
        {
            Ok = true,
            Data = new {typeResult.Value.Id, typeResult.Value.Name}
        });
    }

     [HttpPatch("ticket.helptopic", Name = nameof(ChangeHelpTopic))]
    public async Task<IActionResult> ChangeHelpTopic(ChangeHelpTopicCommand command)
    {
        const string msg = $"PATCH::{nameof(TicketController)}::{nameof(ChangeHelpTopic)}";
        logger.LogInformation(msg);

        command.CreatedBy = Request.HttpContext.UserId();
        var result = await mediatr.Send(command);

        if (result.IsFailed)
            return BadRequest(new ErrorResult
            {
                Ok = false,
                Errors = result.Reasons.Select(s => s.Message)
            });

        var topicResult = await mediatr.Send(new GetHelpTopicCommand{Id = command.HelpTopic});
        if (topicResult.IsFailed)
            return   NotFound(new ErrorResult
            {
                Ok = false,
                Errors = topicResult.Reasons.Select(s=> s.Message)
            });    

        return Ok(new SingleResult<object>
        {
            Ok = true,
            Data = new {topicResult.Value.Id, topicResult.Value.Name}
        });
    }
}