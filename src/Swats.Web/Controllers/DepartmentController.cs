using MediatR;

namespace Swats.Web.Controllers;

public class DepartmentController : MethodController
{
    private readonly ILogger<DepartmentController> logger;
    private readonly IMediator mediatr;

    public DepartmentController(ILogger<DepartmentController> logger, IMediator mediatr)
    {
        this.logger = logger;
        this.mediatr = mediatr;
    }
}