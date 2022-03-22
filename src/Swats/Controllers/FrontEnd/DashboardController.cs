using Microsoft.AspNetCore.Mvc;

namespace Swats.Controllers.FrontEnd;

public class DashboardController : FrontEndController
{
    private readonly ILogger<DashboardController> _logger;

    public DashboardController(ILogger<DashboardController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }
}