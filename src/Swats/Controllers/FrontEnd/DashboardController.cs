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
        _logger.LogInformation($"{Request.Headers["User-Agent"]}");
        
        return View();
    }
}