using Htmx;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Swats.Controllers;
using Swats.Extensions;
using Swats.Model;
using Swats.Model.Commands;
using System.Security.Claims;

namespace Swats.Areas.Admin.Controllers;

[Area("admin")]
public class HelpTopicController : FrontEndController
{
    private readonly IHttpContextAccessor _httpAccessor;
    private readonly ILogger<TicketTypeController> _logger;
    private readonly IMediator _mediatr;

    public HelpTopicController(IHttpContextAccessor httpAccessor
        , ILogger<TicketTypeController> logger
        , IMediator mediatr)
    {
        _httpAccessor = httpAccessor;
        _logger = logger;
        _mediatr = mediatr;
    }

    #region GET

    public async Task<IActionResult> Index()
    {
        _logger.LogInformation($"{Request.Method}::{nameof(HelpTopicController)}::{nameof(Index)}");

        var query = new ListHelpTopicsCommand { };
        var result = await _mediatr.Send(query);
        if (result.IsFailed)
        {
            return NotFound(result.Reasons.FirstOrDefault()?.Message);
        }

        return Request.IsHtmx()
                ? PartialView("~/Areas/Admin/Views/HelpTopic/_Index.cshtml", result.Value)
                : View(result.Value);
    }

    public async Task<IActionResult> Edit(string id)
    {
        _logger.LogInformation($"{Request.Method}::{nameof(HelpTopicController)}::{nameof(Edit)}");

        var query = new GetHelpTopicCommand { Id = id };
        var result = await _mediatr.Send(query);

        if (result.IsFailed)
        {
            return NotFound(result.Reasons.FirstOrDefault()?.Message);
        }
        result.Value.ImageCode = $"{Request.Scheme}://{Request.Host}/admin/helptopic/edit/{id}".GenerateQrCode();

        return Request.IsHtmx()
                ? PartialView("~/Areas/Admin/Views/helptopic/_Edit.cshtml", result.Value)
                : View(result.Value);
    }

    public async Task<IActionResult> Create()
    {
        _logger.LogInformation($"{Request.Method}::{nameof(HelpTopicController)}::{nameof(Create)}");

        var departmentResult = await _mediatr.Send(new ListDepartmentCommand { });
        if (departmentResult.IsFailed)
        {
            return BadRequest(departmentResult.Reasons.FirstOrDefault()?.Message);
        }

        CreateHelpTopicCommand command = new()
        {
            DepartmentList = departmentResult.Value.Select(s => new SelectListItem { Text = s.Name, Value = s.Id })
        };

        return Request.IsHtmx()
             ? PartialView("~/Areas/Admin/Views/HelpTopic/_Create.cshtml", command)
             : View(command);
    }

    #endregion

    #region POST

    [HttpPost]
    public async Task<IActionResult> Create(CreateHelpTopicCommand command)
    {
        var msg = $"{Request.Method}::{nameof(HelpTopicController)}::{nameof(Create)}";
        _logger.LogInformation(msg);

        // get login user id
        var userId = _httpAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

        if (!ModelState.IsValid)
        {
            _logger.LogError($"{msg} - Invalid Model state");

            return Request.IsHtmx()
                ? PartialView("~/Areas/Admin/Views/HelpTopic/_Create.cshtml", command)
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
                ? PartialView("~/Areas/Admin/Views/HelpTopic/_Create.cshtml", command)
                : View(command);
        }

        return RedirectToAction("Edit", new { Id = result.Value });
    }

    #endregion
}
