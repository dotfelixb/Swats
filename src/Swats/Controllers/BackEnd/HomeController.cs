using Microsoft.AspNetCore.Mvc;

namespace Swats.Controllers.BackEnd;

public class HomeController : BackEndController
{
    public IActionResult Index()
    {
        return Ok(new { version = 1, app = "Swats Api" });
    }
}