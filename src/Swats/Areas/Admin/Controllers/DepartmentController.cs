using Htmx;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Swats.Controllers;
using Swats.Extensions;
using Swats.Model.Commands;
using System.Security.Claims;

namespace Swats.Areas.Admin.Controllers;

[Area("admin")]
public class DepartmentController : FrontEndController
{
    private readonly ILogger<TicketTypeController> _logger;
    private readonly IMediator _mediatr;

    public DepartmentController(IHttpContextAccessor httpAccessor
        , ILogger<TicketTypeController> logger
        , IMediator mediatr) : base(httpAccessor)
    {
        _logger = logger;
        _mediatr = mediatr;
    }

    #region POST

    [HttpPost]
    public async Task<IActionResult> Create(CreateDepartmentCommand command)
    {
        var msg = $"{Request.Method}::{nameof(DepartmentController)}::{nameof(Create)}";
        _logger.LogInformation(msg);

        if (!ModelState.IsValid)
        {
            _logger.LogError($"{msg} - Invalid Model state");

            return Request.IsHtmx()
                ? PartialView("~/Areas/Admin/Views/Department/_Create.cshtml", command)
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
                ? PartialView("~/Areas/Admin/Views/Department/_Create.cshtml", command)
                : View(command);
        }

        return RedirectToAction("Edit", new { Id = result.Value });
    }

    #endregion POST

    #region GET

    public async Task<IActionResult> Index()
    {
        _logger.LogInformation($"{Request.Method}::{nameof(DepartmentController)}::{nameof(Index)}");

        var query = new ListDepartmentCommand();
        var result = await _mediatr.Send(query);
        if (result.IsFailed) return NotFound(result.Reasons.FirstOrDefault()?.Message);

        return Request.IsHtmx()
            ? PartialView("~/Areas/Admin/Views/Department/_Index.cshtml", result.Value)
            : View(result.Value);
    }

    public async Task<IActionResult> Edit(string id)
    {
        _logger.LogInformation($"{Request.Method}::{nameof(DepartmentController)}::{nameof(Edit)}");

        var query = new GetDepartmentCommand { Id = id };
        var result = await _mediatr.Send(query);

        if (result.IsFailed) return NotFound(result.Reasons.FirstOrDefault()?.Message);
        result.Value.ImageCode = $"{Request.Scheme}://{Request.Host}/admin/department/edit/{id}".GenerateQrCode();

        return Request.IsHtmx()
            ? PartialView("~/Areas/Admin/Views/department/_Edit.cshtml", result.Value)
            : View(result.Value);
    }

    public async Task<IActionResult> Create()
    {
        _logger.LogInformation($"{Request.Method}::{nameof(DepartmentController)}::{nameof(Create)}");

        var businesshourResult = await _mediatr.Send(new ListBusinessHourCommand());
        if (businesshourResult.IsFailed) return BadRequest(businesshourResult.Reasons.FirstOrDefault()?.Message);

        CreateDepartmentCommand command = new()
        {
            BusinessHours =
                businesshourResult.Value.Select(s => new SelectListItem { Text = s.Name, Value = s.Id.ToString() })
        };

        return Request.IsHtmx()
            ? PartialView("~/Areas/Admin/Views/Department/_Create.cshtml", command)
            : View(command);
    }

    #endregion GET
}