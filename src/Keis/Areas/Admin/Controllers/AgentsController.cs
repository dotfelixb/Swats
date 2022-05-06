using AutoMapper;
using Htmx;
using Keis.Controllers;
using Keis.Extensions;
using Keis.Infrastructure.Features.Agents.CreateAgent;
using Keis.Infrastructure.Features.Agents.GetAgent;
using Keis.Infrastructure.Features.Agents.ListAgent;
using Keis.Infrastructure.Features.Department.ListDepartment;
using Keis.Infrastructure.Features.Teams.ListTeams;
using Keis.Infrastructure.Features.TicketTypes.ListTicketType;
using Keis.Model.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Keis.Areas.Admin.Controllers;

[Area("admin")]
public class AgentsController : FrontEndController
{
    private readonly ILogger<AgentsController> _logger;
    private readonly IMapper _mapper;
    private readonly IMediator _mediatr;
    private readonly UserManager<AuthUser> _userManager;

    public AgentsController(IHttpContextAccessor httpAccessor
        , ILogger<AgentsController> logger
        , IMediator mediatr
        , IMapper mapper
        , UserManager<AuthUser> userManager) : base(httpAccessor)
    {
        _logger = logger;
        _mediatr = mediatr;
        _mapper = mapper;
        _userManager = userManager;
    }

    #region POST

    [HttpPost]
    public async Task<IActionResult> Create(CreateAgentCommand command)
    {
        const string msg = $"POST::{nameof(AgentsController)}::{nameof(Create)}";
        _logger.LogInformation(msg);

        if (!ModelState.IsValid)
        {
            _logger.LogError($"{msg} - Invalid Model state");

            return Request.IsHtmx()
                ? PartialView("~/Areas/Admin/Views/Agents/_Create.cshtml", command)
                : View(command);
        }

        command.CreatedBy = UserId;

        // get user name from email
        command.UserName = command.Email.Split('@').First(); // lisa.paige@keis.app => lisa.paige

        // create a user
        _logger.LogInformation($"{msg} - Creating user for agent");

        // email before at '@' symbol and @1
        var password = $"{command.UserName}@1";
        var user = _mapper.Map<CreateAgentCommand, AuthUser>(command);

        var userResult = await _userManager.CreateAsync(user, password);
        if (!userResult.Succeeded)
        {
            var reason = userResult.Errors.FirstOrDefault()?.Description;
            _logger.LogError($"{msg} - Creating user for agent failed with {reason}");
            TempData["CreateError"] = reason;

            return Request.IsHtmx()
                ? PartialView("~/Areas/Admin/Views/Agents/_Create.cshtml", command)
                : View(command);
        }

        // create agent
        var result = await _mediatr.Send(command);
        if (result.IsFailed)
        {
            var reason = result.Reasons.FirstOrDefault()?.Message ?? "CreateError";
            _logger.LogError($"{msg} - {reason}");
            TempData["CreateError"] = reason;

            return Request.IsHtmx()
                ? PartialView("~/Areas/Admin/Views/Agents/_Create.cshtml", command)
                : View(command);
        }

        return RedirectToAction("Edit", new {Id = result.Value});
    }

    #endregion POST

    #region GET

    public async Task<IActionResult> Index()
    {
        _logger.LogInformation($"{Request.Method}::{nameof(AgentsController)}::{nameof(Index)}");

        var query = new ListAgentCommand();
        var result = await _mediatr.Send(query);
        if (result.IsFailed) return NotFound(result.Reasons.FirstOrDefault()?.Message);

        return Request.IsHtmx()
            ? PartialView("~/Areas/Admin/Views/Agents/_Index.cshtml", result.Value)
            : View(result.Value);
    }

    public async Task<IActionResult> Edit(string id)
    {
        _logger.LogInformation($"{Request.Method}::{nameof(AgentsController)}::{nameof(Index)}");

        var query = new GetAgentCommand {Id = id};
        var result = await _mediatr.Send(query);
        if (result.IsFailed) return NotFound(result.Reasons.FirstOrDefault()?.Message);

        result.Value.ImageCode = $"{Request.Scheme}://{Request.Host}/admin/agents/edit/{id}".GenerateQrCode();

        return Request.IsHtmx()
            ? PartialView("~/Areas/Admin/Views/Agents/_Edit.cshtml", result.Value)
            : View(result.Value);
    }

    public async Task<IActionResult> Create()
    {
        _logger.LogInformation($"{Request.Method}::{nameof(AgentsController)}::{nameof(Create)}");

        var ticketypeResult = await _mediatr.Send(new ListTicketTypeCommand());
        if (ticketypeResult.IsFailed) return BadRequest(ticketypeResult.Reasons.FirstOrDefault()?.Message);

        var departmentList = await _mediatr.Send(new ListDepartmentCommand());
        if (departmentList.IsFailed) return BadRequest(departmentList.Reasons.FirstOrDefault()?.Message);

        var teamList = await _mediatr.Send(new ListTeamsCommand());
        if (teamList.IsFailed) return BadRequest(departmentList.Reasons.FirstOrDefault()?.Message);

        CreateAgentCommand command = new()
        {
            DepartmentList =
                departmentList.Value.Select(s => new SelectListItem {Text = s.Name, Value = s.Id.ToString()}),
            TeamList = teamList.Value.Select(s => new SelectListItem {Text = s.Name, Value = s.Id.ToString()}),
            TypeList = ticketypeResult.Value.Select(s => new SelectListItem {Text = s.Name, Value = s.Id.ToString()})
        };

        return Request.IsHtmx()
            ? PartialView("~/Areas/Admin/Views/Agents/_Create.cshtml", command)
            : View(command);
    }

    #endregion GET
}