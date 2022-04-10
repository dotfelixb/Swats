using Htmx;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Keis.Extensions;
using Keis.Model.Commands;
using System.Security.Claims;

namespace Keis.Controllers.FrontEnd;

public class UserController : FrontEndController
{
    private readonly ILogger<TicketController> _logger;
    private readonly IMediator _mediatr;

    public UserController(IHttpContextAccessor httpAccessor
        , ILogger<TicketController> logger
        , IMediator mediatr) : base(httpAccessor)
    {
        _logger = logger;
        _mediatr = mediatr;
    }

    public async Task<IActionResult> Index()
    {
        var result = await _mediatr.Send(new GetUserCommand { Id = UserId });
        if (result.IsFailed) return NotFound(result.Reasons.FirstOrDefault()?.Message);

        result.Value.ImageCode = $"{Request.Scheme}://{Request.Host}/user/{UserId}".GenerateQrCode();

        return Request.IsHtmx()
            ? PartialView("~/Views/User/_Index.cshtml", result.Value)
            : View(result.Value);
    }
}