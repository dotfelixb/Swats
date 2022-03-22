using System.Security.Claims;
using Htmx;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swats.Controllers;
using Swats.Extensions;
using Swats.Model.Commands;

namespace Swats.Areas.Admin.Controllers;

[Area("admin")]
public class TagsController : FrontEndController
{
    private readonly IHttpContextAccessor _httpAccessor;
    private readonly ILogger<TicketTypeController> _logger;
    private readonly IMediator _mediatr;

    public TagsController(IHttpContextAccessor httpAccessor
        , ILogger<TicketTypeController> logger
        , IMediator mediatr)
    {
        _httpAccessor = httpAccessor;
        _logger = logger;
        _mediatr = mediatr;
    }

    #region POST

    [HttpPost]
    public async Task<IActionResult> Create(CreateTagCommand command)
    {
        var msg = $"{Request.Method}::{nameof(TagsController)}::{nameof(Create)}";
        _logger.LogInformation(msg);

        // get login user id
        var userId = _httpAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

        if (!ModelState.IsValid)
        {
            _logger.LogError($"{msg} - Invalid Model state");

            return Request.IsHtmx()
                ? PartialView("~/Areas/Admin/Views/Tags/_Create.cshtml", command)
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
                ? PartialView("~/Areas/Admin/Views/Tags/_Create.cshtml", command)
                : View(command);
        }

        return RedirectToAction("Edit", new {Id = result.Value});
    }

    #endregion

    #region GET

    public async Task<IActionResult> Index()
    {
        _logger.LogInformation($"{Request.Method}::{nameof(TagsController)}::{nameof(Index)}");

        var query = new ListTagsCommand();
        var result = await _mediatr.Send(query);
        if (result.IsFailed) return NotFound(result.Reasons.FirstOrDefault()?.Message);

        return Request.IsHtmx()
            ? PartialView("~/Areas/Admin/Views/Tags/_Index.cshtml", result.Value)
            : View(result.Value);
    }

    public async Task<IActionResult> Edit(string id)
    {
        _logger.LogInformation($"{Request.Method}::{nameof(TagsController)}::{nameof(Edit)}");

        var query = new GetTagCommand {Id = id};
        var result = await _mediatr.Send(query);
        if (result.IsFailed) return NotFound(result.Reasons.FirstOrDefault()?.Message);

        result.Value.ImageCode = $"{Request.Scheme}://{Request.Host}/admin/tags/edit/{id}".GenerateQrCode();

        return Request.IsHtmx()
            ? PartialView("~/Areas/Admin/Views/Tags/_Edit.cshtml", result.Value)
            : View(result.Value);
    }

    public IActionResult Create()
    {
        _logger.LogInformation($"{Request.Method}::{nameof(TagsController)}::{nameof(Create)}");

        return Request.IsHtmx()
            ? PartialView("~/Areas/Admin/Views/Tags/_Create.cshtml")
            : View();
    }

    #endregion
}