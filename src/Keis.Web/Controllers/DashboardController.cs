using Keis.Infrastructure.Features.Dashboard.CountDashboard;
using Keis.Model;
using Keis.Model.Queries;
using Keis.Web.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Keis.Web.Controllers;

public class DashboardController : MethodController
{
    private readonly ILogger<DashboardController> logger;
    private readonly IMediator mediatr;

    public DashboardController(ILogger<DashboardController> logger, IMediator mediatr)
    {
        this.logger = logger;
        this.mediatr = mediatr;
    }

    [HttpGet("dashboard.count", Name = nameof(CountDashboard))]
    public async Task<IActionResult> CountDashboard()
    {
        const string msg = $"GET::{nameof(DashboardController)}::{nameof(CountDashboard)}";
        logger.LogInformation(msg);
        
        var userId = Request.HttpContext.UserId();
        var countResult = await mediatr.Send(new CountDashboardCommand{ Id = userId});
        if (countResult.IsFailed)
            return BadRequest(new ErrorResult
            {
                Ok = false,
                Errors = countResult.Reasons.Select(s => s.Message)
            });
        
        return Ok(new SingleResult<DashboardCount>
        {
            Ok = true,
            Data = countResult.Value
        });
    }
}