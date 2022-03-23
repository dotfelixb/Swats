using Microsoft.AspNetCore.Mvc;
using Swats.Controllers;
using Swats.Model.ViewModel;

namespace Swats.Areas.Admin.Controllers;

[Area("admin")]
public class UserController : FrontEndController
{
    public UserController(IHttpContextAccessor httpAccessor) : base(httpAccessor)
    {
    }
    
    public IActionResult Index()
    {
        var partial = new IndexPartial
        {
            CreateLocation = "/admin/user/create",
            CreateTitle = "New User",
            Title = "Users"
        };
        return View(partial);
    }
}