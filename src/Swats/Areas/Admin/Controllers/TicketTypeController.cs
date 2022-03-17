﻿using Htmx;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swats.Controllers;
using Swats.Extensions;
using Swats.Model.Commands;
using Swats.Model.Domain;
using Swats.Model.Queries;
using Swats.Model.ViewModel;
using System.Security.Claims;
using System.Text.Json;

namespace Swats.Areas.Admin.Controllers;

[Area("admin")]
public class TicketTypeController : FrontEndController
{
    private readonly IHttpContextAccessor _httpAccessor;
    private readonly ILogger<TicketTypeController> _logger;
    private readonly IMediator _mediatr;

    public TicketTypeController(IHttpContextAccessor contextAccessor
        , ILogger<TicketTypeController> logger
        , IMediator mediatr)
    {
        ;
        _httpAccessor = contextAccessor;
        _logger = logger;
        _mediatr = mediatr;
    }

    public async Task<IActionResult> Index()
    {
        _logger.LogInformation($"{Request.Method}::{nameof(TicketTypeController)}::{nameof(Index)}");
        var query = new ListTicketTypeCommand{};
        var result = await _mediatr.Send(query);
        if(result.IsFailed)
        {
            return NotFound(result.Reasons.FirstOrDefault()?.Message);
        }
        
        return Request.IsHtmx()
                ? PartialView("~/Areas/Admin/Views/_Index.cshtml",result.Value)
                : View(result.Value);
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        var query = new GetTicketTypeCommand { Id = id };
        var result = await _mediatr.Send(query);

        if (result.IsFailed)
        {
            return NotFound(result.Reasons.FirstOrDefault()?.Message);
        }

        return Request.IsHtmx()
                ? PartialView("~/Areas/Admin/Views/TicketType/_Edit.cshtml", result.Value)
                : View(result.Value);
    }

    public IActionResult Create()
    {
        _logger.LogInformation($"{Request.Method}::{nameof(TicketTypeController)}::{nameof(Create)}");

        return Request.IsHtmx()
             ? PartialView("~/Areas/Admin/Views/TicketType/_Create.cshtml")
             : View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateTicketTypeCommand command)
    {
        // get login user id
        var userId = _httpAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        
        var msg = $"{Request.Method}::{nameof(TicketTypeController)}::{nameof(Create)}";
        _logger.LogInformation(msg);

        if (!ModelState.IsValid)
        {
            return Request.IsHtmx()
                ? PartialView("~/Areas/Admin/Views/TicketType/_Create.cshtml", command)
                : View(command);
        }

        command.CreatedBy = userId.ToGuid();
        var result = await _mediatr.Send(command);
        if (result.IsFailed)
        {
            _logger.LogInformation($"{msg} - Unable to get login user");
            TempData["CreateError"] = result.Reasons.FirstOrDefault()?.Message ?? "CreateError";

            return Request.IsHtmx()
               ? PartialView("~/Areas/Admin/Views/TicketType/_Create.cshtml", command)
               : View(command);
        }

        return RedirectToAction("Edit", new { Id = result.Value});
    }
}
