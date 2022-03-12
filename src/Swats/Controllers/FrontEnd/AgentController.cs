using Microsoft.AspNetCore.Mvc;

namespace Swats.Controllers.FrontEnd;

public class AgentController : FrontEndController
{
    public IActionResult Index()
    {
        return View();
    }
}