using AutoMapper;
using Htmx;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Swats.Controllers;
using Swats.Extensions;
using Swats.Model;
using Swats.Model.Commands;
using Swats.Model.Domain;
using Swats.Model.Queries;
using System.Security.Claims;

namespace Swats.Areas.Admin.Controllers;

[Area("admin")]
public class AgentsController : FrontEndController
{
    private readonly IHttpContextAccessor _httpAccessor;
    private readonly ILogger<AgentsController> _logger;
    private readonly IMediator _mediatr;
    private readonly IMapper _mapper;
    private readonly UserManager<AuthUser> _userManager;

    public AgentsController(IHttpContextAccessor httpAccessor
        , ILogger<AgentsController> logger
        , IMediator mediatr
        , IMapper mapper
        , UserManager<AuthUser> userManager)
    {
        _logger = logger;
        _httpAccessor = httpAccessor;
        _mediatr = mediatr;
        _mapper = mapper;
        _userManager = userManager;
    }

    #region GET

    public IActionResult Index()
    {
        _logger.LogInformation($"{Request.Method}::{nameof(AgentsController)}::{nameof(Index)}");

        var result = Enumerable.Empty<FetchedAgent>();

        return Request.IsHtmx()
                ? PartialView("~/Areas/Admin/Views/Agents/_Index.cshtml", result)
                : View(result);
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        _logger.LogInformation($"{Request.Method}::{nameof(AgentsController)}::{nameof(Index)}");

        var query = new GetAgentCommand { Id = id };
        var result = await _mediatr.Send(query);
        if (result.IsFailed)
        {
            return NotFound(result.Reasons.FirstOrDefault()?.Message);
        }
        result.Value.ImageCode = $"{Request.Scheme}://{Request.Host}/admin/agent/edit/{id}".GenerateQrCode();

        return Request.IsHtmx()
                ? PartialView("~/Areas/Admin/Views/Agent/_Edit.cshtml", result.Value)
                : View(result.Value);
    }

    public async Task<IActionResult> Create()
    {
        _logger.LogInformation($"{Request.Method}::{nameof(AgentsController)}::{nameof(Create)}");

        var ticketypeResult = await _mediatr.Send(new ListTicketTypeCommand { });
        if (ticketypeResult.IsFailed)
        {
            return BadRequest(ticketypeResult.Reasons.FirstOrDefault()?.Message);
        }

        CreateAgentCommand command = new()
        {
            TypeList = ticketypeResult.Value.Select(s => new SelectListItem { Text = s.Name, Value = s.Id.ToString() })
        };

        return Request.IsHtmx()
             ? PartialView("~/Areas/Admin/Views/Agents/_Create.cshtml", command)
             : View(command);
    }

    #endregion GET

    #region POST

    [HttpPost]
    public async Task<IActionResult> Create(CreateAgentCommand command)
    {
        var msg = $"{Request.Method}::{nameof(AgentsController)}::{nameof(Create)}";
        _logger.LogInformation(msg);

        // get login user id
        var userId = _httpAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

        if (!ModelState.IsValid)
        {
            _logger.LogError($"{msg} - Invalid Model state");

            return Request.IsHtmx()
             ? PartialView("~/Areas/Admin/Views/Agents/_Create.cshtml", command)
             : View(command);
        }

        command.CreatedBy = userId.ToGuid();

        // get user name from email
        command.UserName = command.Email.Split('@').First(); // lisa.paige@swats.app => lisa.paige

        // create a user
        _logger.LogInformation($"{msg} - Createing user for agent");

        var user = _mapper.Map<CreateAgentCommand, AuthUser>(command);
        var userResult = await _userManager.CreateAsync(user, command.UserName);
        if (!userResult.Succeeded)
        {
            var reason = userResult.Errors.FirstOrDefault()?.Description;
            _logger.LogError($"{msg} - Createing user for agent failed with {reason}");
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

        return RedirectToAction("Edit", new { Id = result.Value });
    }

    #endregion POST
}