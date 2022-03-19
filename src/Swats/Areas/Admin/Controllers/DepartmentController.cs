using Htmx;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Swats.Controllers;
using Swats.Model.Commands;
using Swats.Model.Queries;
using Swats.Model.ViewModel;

namespace Swats.Areas.Admin.Controllers;

[Area("admin")]
public class DepartmentController : FrontEndController
{
    private readonly IHttpContextAccessor _httpAccessor;
    private readonly ILogger<TicketTypeController> _logger;
    private readonly IMediator _mediatr;

    public DepartmentController(IHttpContextAccessor httpAccessor
        , ILogger<TicketTypeController> logger
        , IMediator mediatr)
    {
        _httpAccessor = httpAccessor;
        _logger = logger;
        _mediatr = mediatr;
    }

    public IActionResult Index()
    {
        _logger.LogInformation($"{Request.Method}::{nameof(DepartmentController)}::{nameof(Index)}");

        var partial = new IndexPartial
        {
            CreateLocation = "/admin/department/create",
            CreateTitle = "New Department",
            Title = "Departments"

        };
        return Request.IsHtmx()
             ? PartialView("~/Areas/Admin/Views/_Index.cshtml",partial)
             : View(partial);
    }

    public async Task<IActionResult> Create()
    {
        _logger.LogInformation($"{Request.Method}::{nameof(DepartmentController)}::{nameof(Create)}");

        var businesshourResult = await _mediatr.Send(new ListBusinessHourCommand {});
        if (businesshourResult.IsFailed)
        {
            return BadRequest(businesshourResult.Reasons.FirstOrDefault()?.Message);
        }

        CreateDepartmentCommand command = new()
        {
            BusinessHours = businesshourResult.Value.Select(s => new SelectListItem{Text = s.Name, Value = s.Id.ToString() })
        };

        return Request.IsHtmx()
             ? PartialView("~/Areas/Admin/Views/Department/_Create.cshtml", command)
             : View(command);
    }
}
