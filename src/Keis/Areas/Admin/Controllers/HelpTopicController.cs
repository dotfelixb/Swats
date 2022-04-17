using Htmx;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Keis.Controllers;
using Keis.Extensions;
using Keis.Model.Commands;
using System.Security.Claims;
using Keis.Infrastructure.Features.Department.ListDepartment;
using Keis.Infrastructure.Features.HelpTopic.CreateHelpTopic;
using Keis.Infrastructure.Features.HelpTopic.GetHelpTopic;
using Keis.Infrastructure.Features.HelpTopic.ListHelpTopics;

namespace Keis.Areas.Admin.Controllers;

[Area("admin")]
public class HelpTopicController : FrontEndController
{
    private readonly ILogger<HelpTopicController> _logger;
    private readonly IMediator _mediatr;

    public HelpTopicController(IHttpContextAccessor httpAccessor
        , ILogger<HelpTopicController> logger
        , IMediator mediatr) :base(httpAccessor)
    {
        _logger = logger;
        _mediatr = mediatr;
    }

    #region POST

    [HttpPost]
    public async Task<IActionResult> Create(CreateHelpTopicCommand command)
    {
        var msg = $"{Request.Method}::{nameof(HelpTopicController)}::{nameof(Create)}";
        _logger.LogInformation(msg);

        if (!ModelState.IsValid)
        {
            _logger.LogError($"{msg} - Invalid Model state");

            return Request.IsHtmx()
                ? PartialView("~/Areas/Admin/Views/HelpTopic/_Create.cshtml", command)
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
                ? PartialView("~/Areas/Admin/Views/HelpTopic/_Create.cshtml", command)
                : View(command);
        }

        return RedirectToAction("Edit", new { Id = result.Value });
    }

    #endregion POST

    #region GET

    public async Task<IActionResult> Index()
    {
        _logger.LogInformation($"{Request.Method}::{nameof(HelpTopicController)}::{nameof(Index)}");

        var query = new ListHelpTopicsCommand();
        var result = await _mediatr.Send(query);
        if (result.IsFailed) return NotFound(result.Reasons.FirstOrDefault()?.Message);

        return Request.IsHtmx()
            ? PartialView("~/Areas/Admin/Views/HelpTopic/_Index.cshtml", result.Value)
            : View(result.Value);
    }

    public async Task<IActionResult> Edit(string id)
    {
        _logger.LogInformation($"{Request.Method}::{nameof(HelpTopicController)}::{nameof(Edit)}");

        var query = new GetHelpTopicCommand { Id = id };
        var result = await _mediatr.Send(query);

        if (result.IsFailed) return NotFound(result.Reasons.FirstOrDefault()?.Message);
        result.Value.ImageCode = $"{Request.Scheme}://{Request.Host}/admin/helptopic/edit/{id}".GenerateQrCode();

        return Request.IsHtmx()
            ? PartialView("~/Areas/Admin/Views/helptopic/_Edit.cshtml", result.Value)
            : View(result.Value);
    }

    public async Task<IActionResult> Create()
    {
        _logger.LogInformation($"{Request.Method}::{nameof(HelpTopicController)}::{nameof(Create)}");

        var departmentList = await _mediatr.Send(new ListDepartmentCommand());
        if (departmentList.IsFailed) return BadRequest(departmentList.Reasons.FirstOrDefault()?.Message);

        CreateHelpTopicCommand command = new()
        {
            DepartmentList = departmentList.Value.Select(s => new SelectListItem { Text = s.Name, Value = s.Id })
        };

        return Request.IsHtmx()
            ? PartialView("~/Areas/Admin/Views/HelpTopic/_Create.cshtml", command)
            : View(command);
    }

    #endregion GET
}