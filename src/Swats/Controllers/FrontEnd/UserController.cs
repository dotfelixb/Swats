using Htmx;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swats.Extensions;
using Swats.Model.Commands;
using System.Security.Claims;

namespace Swats.Controllers.FrontEnd;

public class UserController : FrontEndController
{
    private readonly IHttpContextAccessor _httpAccessor;
    private readonly ILogger<TicketController> _logger;
    private readonly IMediator _mediatr;

    public UserController(IHttpContextAccessor httpAccessor
        , ILogger<TicketController> logger
        , IMediator mediatr)
    {
        _httpAccessor = httpAccessor;
        _logger = logger;
        _mediatr = mediatr;
    }

    public async Task<IActionResult> Index()
    {
        // get login user id
        var userId = _httpAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

        var result = await _mediatr.Send(new GetUserCommand { Id = userId });
        if (result.IsFailed) return NotFound(result.Reasons.FirstOrDefault()?.Message);

        result.Value.ImageCode = $"{Request.Scheme}://{Request.Host}/user/{userId}".GenerateQrCode();

        return Request.IsHtmx()
            ? PartialView("~/Views/User/_Index.cshtml", result.Value)
            : View(result.Value);
    }
}