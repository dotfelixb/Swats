using Microsoft.AspNetCore.Mvc;
using Swats.Controllers;
using Swats.Model.ViewModel;

namespace Swats.Areas.Admin.Controllers;

[Area("admin")]
public class AgentsController : FrontEndController
{
    public IActionResult Index()
    {
        var partial = new IndexPartial
        {
            CreateLocation = "/admin/agents/create",
            CreateTitle = "New Agent",
            Title = "Agents"
        };
        return View(partial);
    }
}