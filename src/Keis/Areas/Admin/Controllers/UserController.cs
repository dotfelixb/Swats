using Keis.Controllers;
using Keis.Model.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Keis.Areas.Admin.Controllers;

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