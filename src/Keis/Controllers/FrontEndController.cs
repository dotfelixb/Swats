using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace Keis.Controllers;

public class FrontEndController : Controller
{
    private readonly IHttpContextAccessor _httpAccessor;

    public FrontEndController(IHttpContextAccessor httpAccessor)
    {
        _httpAccessor = httpAccessor;
        UserId = _httpAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
    }

    public string UserId { get; }
}