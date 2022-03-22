using System.Security.Claims;
using Htmx;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Swats.Controllers;
using Swats.Extensions;
using Swats.Model.Commands;

namespace Swats.Areas.Admin.Controllers;

[Area("admin")]
public class TeamsController : FrontEndController
{
    private readonly IHttpContextAccessor _httpAccessor;
    private readonly ILogger<TicketTypeController> _logger;
    private readonly IMediator _mediatr;

    public TeamsController(IHttpContextAccessor httpAccessor
        , ILogger<TicketTypeController> logger
        , IMediator mediatr)
    {
        _httpAccessor = httpAccessor;
        _logger = logger;
        _mediatr = mediatr;
    }

    #region POST

    [HttpPost]
    public async Task<IActionResult> Create(CreateTeamCommand command)
    {
        var msg = $"{Request.Method}::{nameof(TeamsController)}::{nameof(Create)}";
        _logger.LogInformation(msg);

        // get login user id
        var userId = _httpAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

        if (!ModelState.IsValid)
        {
            _logger.LogError($"{msg} - Invalid Model state");

            return Request.IsHtmx()
                ? PartialView("~/Areas/Admin/Views/Teams/_Create.cshtml", command)
                : View(command);
        }

        command.CreatedBy = userId;
        var result = await _mediatr.Send(command);
        if (result.IsFailed)
        {
            var reason = result.Reasons.FirstOrDefault()?.Message ?? "CreateError";
            _logger.LogError($"{msg} - {reason}");
            TempData["CreateError"] = reason;

            return Request.IsHtmx()
                ? PartialView("~/Areas/Admin/Views/Teams/_Create.cshtml", command)
                : View(command);
        }

        return RedirectToAction("Edit", new {Id = result.Value});
    }

    #endregion POST

    #region GET

    public async Task<IActionResult> Index()
    {
        _logger.LogInformation($"{Request.Method}::{nameof(TeamsController)}::{nameof(Index)}");

        var query = new ListTeamsCommand();
        var result = await _mediatr.Send(query);
        if (result.IsFailed) return NotFound(result.Reasons.FirstOrDefault()?.Message);

        return Request.IsHtmx()
            ? PartialView("~/Areas/Admin/Views/Teams/_Index.cshtml", result.Value)
            : View(result.Value);
    }

    public async Task<IActionResult> Edit(string id)
    {
        _logger.LogInformation($"{Request.Method}::{nameof(TeamsController)}::{nameof(Edit)}");

        var query = new GetTeamCommand {Id = id};
        var result = await _mediatr.Send(query);

        if (result.IsFailed) return NotFound(result.Reasons.FirstOrDefault()?.Message);
        result.Value.ImageCode = $"{Request.Scheme}://{Request.Host}/admin/teams/edit/{id}".GenerateQrCode();

        return Request.IsHtmx()
            ? PartialView("~/Areas/Admin/Views/Teams/_Edit.cshtml", result.Value)
            : View(result.Value);
    }

    public async Task<IActionResult> Create()
    {
        _logger.LogInformation($"{Request.Method}::{nameof(TeamsController)}::{nameof(Create)}");

        var departmentList = await _mediatr.Send(new ListDepartmentCommand());
        if (departmentList.IsFailed) return BadRequest(departmentList.Reasons.FirstOrDefault()?.Message);

        CreateTeamCommand command = new()
        {
            DepartmentList = departmentList.Value.Select(s => new SelectListItem {Text = s.Name, Value = s.Id})
        };

        return Request.IsHtmx()
            ? PartialView("~/Areas/Admin/Views/Teams/_Create.cshtml", command)
            : View(command);
    }

    #endregion GET
}