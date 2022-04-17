using Htmx;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Keis.Extensions;
using Keis.Infrastructure.Features.Agents.GetAgent;
using Keis.Infrastructure.Features.Agents.ListAgent;
using Keis.Infrastructure.Features.Department.ListDepartment;
using Keis.Infrastructure.Features.HelpTopic.ListHelpTopics;
using Keis.Infrastructure.Features.Teams.ListTeams;
using Keis.Infrastructure.Features.Tickets.CreateComment;
using Keis.Infrastructure.Features.Tickets.CreateTicket;
using Keis.Infrastructure.Features.Tickets.GetTicket;
using Keis.Infrastructure.Features.Tickets.ListComments;
using Keis.Infrastructure.Features.Tickets.ListTicket;
using Keis.Infrastructure.Features.TicketTypes.ListTicketType;
using Keis.Model.Commands;
using Keis.Model.Queries;

namespace Keis.Controllers.FrontEnd;

public class TicketController : FrontEndController
{
    private readonly ILogger<TicketController> _logger;
    private readonly IMediator _mediatr;

    public TicketController(IHttpContextAccessor httpAccessor
        , ILogger<TicketController> logger
        , IMediator mediatr) : base(httpAccessor)
    {
        _logger = logger;
        _mediatr = mediatr;
    }

    #region GET

    public async Task<IActionResult> Index()
    {
        _logger.LogInformation($"{Request.Method}::{nameof(TicketController)}::{nameof(Index)}");

        var query = new ListTicketCommand {Id = UserId, IncludeDepartment = true};
        var result = await _mediatr.Send(query);
        if (result.IsFailed) return NotFound(result.Reasons.FirstOrDefault()?.Message);

        return Request.IsHtmx()
            ? PartialView("~/Views/Ticket/_Index.cshtml", result.Value.Data)
            : View(result.Value.Data);
    }

    public async Task<IActionResult> Edit(string id)
    {
        _logger.LogInformation($"{Request.Method}::{nameof(TicketController)}::{nameof(Edit)}");

        var query = new GetTicketCommand {Id = id};
        var result = await _mediatr.Send(query);

        if (result.IsFailed) return NotFound(result.Reasons.FirstOrDefault()?.Message);
        result.Value.ImageCode = $"{Request.Scheme}://{Request.Host}/ticket/edit/{id}".GenerateQrCode();

        var commentResult = await _mediatr.Send(new ListTicketCommentCommand {TicketId = id});
        if (commentResult.IsSuccess)
        {
            result.Value.TicketComments = commentResult.Value;
        }
        
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
                {Text = $"{s.FirstName} {s.LastName}", Value = s.Id.ToString()}),
            RequesterList = requesterList.Value.Select(s => new SelectListItem
                {Text = $"{s.FirstName} {s.LastName}", Value = s.Id.ToString()}),
            DepartmentList = departmentList.Value.Select(s => new SelectListItem
                {Text = s.Name, Value = s.Id.ToString()}),
            TeamList = teamList.Value.Select(s => new SelectListItem
                {Text = s.Name, Value = s.Id.ToString()}),
            TypeList = ticketTypeList.Value.Select(s => new SelectListItem
                {Text = s.Name, Value = s.Id.ToString()}),
            HelpTopicList = helptopicList.Value.Select(s => new SelectListItem
                {Text = s.Topic, Value = s.Id.ToString()})
        };

        return Request.IsHtmx()
            ? PartialView("~/Views/Ticket/_Create.cshtml", command)
            : View(command);
    }

    public IActionResult AddComment([FromQuery]string comment, [FromQuery]string ticket)
    {
        var command = new CreateTicketCommentCommand
        {
            CommentId = comment,
            TicketId = ticket
        };
        
        return PartialView("~/Views/Ticket/_AddComment.cshtml", command);
    }

    public IActionResult AddReply([FromQuery] string comment, [FromQuery]string ticket)
    {
        var command = new CreateTicketReplyCommand
        {
            CommentId = comment,
            TicketId = ticket
        };
        
        return PartialView("~/Views/Ticket/_AddReply.cshtml", command);
    }

    public async Task<IActionResult> ListComments(string id)
    {
        var msg = $"{Request.Method}::{nameof(TicketController)}::{nameof(ListComments)}";
        _logger.LogInformation(msg);

        var ticket = Enumerable.Empty<FetchTicketComment>();
        
        var commentResult = await _mediatr.Send(new ListTicketCommentCommand {TicketId = id});
        if (commentResult.IsSuccess)
        {
            ticket = commentResult.Value;
        }

        return PartialView("~/Views/Ticket/_ListComments.cshtml", ticket);
    }

    public IActionResult Cancel()
    {
        return PartialView("~/Views/Ticket/_Cancel.cshtml");
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
        
        //get requester details
        var agentResult =await  _mediatr.Send(new GetAgentCommand {Id = command.Requester});
        if (agentResult.IsSuccess)
        {
            command.RequesterEmail = agentResult.Value.Email;
            command.RequesterName = $"{agentResult.Value.FirstName} {agentResult.Value.LastName}";
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

        return RedirectToAction("Edit", new {Id = result.Value});
    }

    [HttpPost]
    public async Task<IActionResult> AddComment(CreateTicketCommentCommand command)
    {
        var msg = $"{Request.Method}::{nameof(TicketController)}::{nameof(Create)}";
        _logger.LogInformation(msg);
        
        if (!ModelState.IsValid)
        {
            _logger.LogError($"{msg} - Invalid Model State");
            TempData["CreateError"] = "Please add your comment";
            
            return PartialView("~/Views/Ticket/_AddComment.cshtml", command);
        }

        var agentResult = await _mediatr.Send(new GetAgentCommand {Id = UserId});
        if (agentResult.IsSuccess)
        {
            command.FromName = $"{agentResult.Value.FirstName} {agentResult.Value.LastName}";
            command.FromEmail = agentResult.Value.Email;
        }

        command.CreatedBy = UserId;
        var result = await _mediatr.Send(command);
        if (result.IsFailed)
        {
            var reason = result.Reasons.FirstOrDefault()?.Message ?? "CreateError";
            _logger.LogError($"{msg} - {reason}");
            TempData["CreateError"] = reason;
            
            return PartialView("~/Views/Ticket/_AddComment.cshtml", command);
        }
        
        return RedirectToAction("ListComments", new{ id = command.TicketId });
    }

    #endregion POST
}