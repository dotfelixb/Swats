using Microsoft.AspNetCore.Mvc;

namespace Keis.Controllers.BackEnd;

public class HomeController : BackEndController
{
    public IActionResult Index()
    {
        return Ok(new { version = 1, app = "Keis Api" });
    }
}