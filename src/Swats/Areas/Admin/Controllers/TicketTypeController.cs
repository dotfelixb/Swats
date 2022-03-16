using Htmx;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swats.Controllers;
using Swats.Model.Commands;
using Swats.Model.Domain;
using Swats.Model.ViewModel;
using System.Text.Json;

namespace Swats.Areas.Admin.Controllers;

[Area("admin")]
public class TicketTypeController : FrontEndController
{
    private readonly UserManager<AuthUser> _userManager;
    private readonly ILogger<TicketTypeController> _logger;
    private readonly IMediator _mediatr;

    public TicketTypeController(UserManager<AuthUser> userManager
        , ILogger<TicketTypeController> logger
        , IMediator mediatr)
    {
        _userManager = userManager;
        _logger = logger;
        _mediatr = mediatr;
    }

    public IActionResult Index()
    {
        _logger.LogInformation($"{Request.Method}::{nameof(TicketTypeController)}::{nameof(Index)}");

        var partial = new IndexPartial
        {
            CreateLocation = "/admin/tickettype/create",
            CreateTitle = "New Ticket Type",
            Title = "Ticket Types"
        };

        return Request.IsHtmx()
                ? PartialView("~/Areas/Admin/Views/_Index.cshtml", partial)
                : View(partial);
    }

    public IActionResult Edit(Guid id)
    {
        return View();
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
        var msg = $"{Request.Method}::{nameof(TicketTypeController)}::{nameof(Create)}";
        _logger.LogInformation(msg);

        if (!ModelState.IsValid)
        {
            return Request.IsHtmx()
                ? PartialView("~/Areas/Admin/Views/TicketType/_Create.cshtml", command)
                : View(command);
        }

        // get login user
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            _logger.LogInformation($"{msg} - Unable to get login user");
            TempData["UserFetchError"] = "Unable to get login user";

            return Request.IsHtmx()
               ? PartialView("~/Areas/Admin/Views/TicketType/_Create.cshtml", command)
               : View(command);
        }
        
        command.CreatedBy = user.Id;
        var result = await _mediatr.Send(command);
        if (result.IsFailed)
        {
            _logger.LogInformation($"{msg} - Unable to get login user");
            TempData["CreateError"] = result.Reasons.FirstOrDefault()?.Message ?? "CreateError";

            return Request.IsHtmx()
               ? PartialView("~/Areas/Admin/Views/TicketType/_Create.cshtml", command)
               : View(command);
        }

        ViewData["EditTitle"] = "<put new type id here>";
        return RedirectToAction("Edit", new { Id = result.Value});
    }
}
