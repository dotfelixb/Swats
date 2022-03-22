using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swats.Model.Commands;
using Swats.Model.ViewModel;
using System.Security.Claims;

namespace Swats.Controllers.FrontEnd;

public class DashboardController : FrontEndController
{
    private readonly IHttpContextAccessor _httpAccessor;
    private readonly ILogger<DashboardController> _logger;
    private readonly IMediator _mediatr;


    public DashboardController(IHttpContextAccessor httpAccessor, ILogger<DashboardController> logger, IMediator mediatr)
    {
        _httpAccessor = httpAccessor;
        _logger = logger;
        _mediatr = mediatr;
    }

    public IActionResult Index()
    {
        _logger.LogInformation($"{Request.Method}::{nameof(DashboardController)}::{nameof(Index)}");

        // get login user id
        var userId = _httpAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

        return View();
    }

    public async Task<IActionResult> Tickets()
    {
        _logger.LogInformation($"{Request.Method}::{nameof(DashboardController)}::{nameof(Tickets)}");
        // get login user id
        var userId = _httpAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

        var result = await _mediatr.Send(new AgentTicketsCommand { Id = userId });
        StatsCard statsCard = new StatsCard { Title = "My Tickets", Location = "/dashboard/tickets" };

        if(result.IsSuccess)
        {
            statsCard.Count = result.Value;
        }
        return PartialView("~/Views/Dashboard/_Tickets.cshtml", statsCard);
    }

    public async Task<IActionResult> Overdue()
    {
        _logger.LogInformation($"{Request.Method}::{nameof(DashboardController)}::{nameof(Overdue)}");
        // get login user id
        var userId = _httpAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

        var result = await _mediatr.Send(new AgentTicketsCommand { Id = userId, OverdueOnly = true });
        StatsCard statsCard = new StatsCard { Title = "My Overdue Tickets", Location = "/dashboard/tickets" };

        if (result.IsSuccess)
        {
            statsCard.Count = result.Value;
        }
        return PartialView("~/Views/Dashboard/_Tickets.cshtml", statsCard);
    }

    public async Task<IActionResult> DueToday()
    {
        _logger.LogInformation($"{Request.Method}::{nameof(DashboardController)}::{nameof(DueToday)}");
        // get login user id
        var userId = _httpAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

        var result = await _mediatr.Send(new AgentTicketsCommand { Id = userId, DueToday = true });
        StatsCard statsCard = new StatsCard { Title = "My due Today", Location = "/dashboard/tickets" };

        if (result.IsSuccess)
        {
            statsCard.Count = result.Value;
        }
        return PartialView("~/Views/Dashboard/_Tickets.cshtml", statsCard);
    }

    public async Task<IActionResult> OpenTickets()
    {
        _logger.LogInformation($"{Request.Method}::{nameof(DashboardController)}::{nameof(OpenTickets)}");
        // get login user id
        var userId = _httpAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

        var result = await _mediatr.Send(new AgentTicketsCommand { Id = userId });
        StatsCard statsCard = new StatsCard { Title = "Open Tickets", Location = "/dashboard/tickets" };

        if (result.IsSuccess)
        {
            statsCard.Count = result.Value;
        }
        return PartialView("~/Views/Dashboard/_Tickets.cshtml", statsCard);
    }

    public async Task<IActionResult> OpenOverdue()
    {
        _logger.LogInformation($"{Request.Method}::{nameof(DashboardController)}::{nameof(OpenOverdue)}");
        // get login user id
        var userId = _httpAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

        var result = await _mediatr.Send(new AgentTicketsCommand { Id = userId });
        StatsCard statsCard = new StatsCard { Title = "Overdue Tickets", Location = "/dashboard/tickets" };

        if (result.IsSuccess)
        {
            statsCard.Count = result.Value;
        }
        return PartialView("~/Views/Dashboard/_Tickets.cshtml", statsCard);
    }
}