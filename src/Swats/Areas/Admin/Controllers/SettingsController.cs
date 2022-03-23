using Htmx;
using Microsoft.AspNetCore.Mvc;
using Swats.Controllers;

namespace Swats.Areas.Admin.Controllers;

[Area("admin")]
public class SettingsController : FrontEndController
{
    private readonly ILogger<SettingsController> _logger;

    public SettingsController(IHttpContextAccessor httpAccessor
        , ILogger<SettingsController> logger) : base(httpAccessor)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        _logger.LogInformation($"{Request.Method}::{nameof(SettingsController)}::{nameof(Index)}");

        return Request.IsHtmx()
            ? PartialView("~/Areas/Admin/Views/Settings/_Index.cshtml")
            : View();
    }
}