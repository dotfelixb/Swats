using MediatR;

namespace Swats.Web.Controllers;

public class BusinessHourController : MethodController
{
    private readonly ILogger<BusinessHourController> logger;
    private readonly IMediator mediatr;

    public BusinessHourController(ILogger<BusinessHourController> logger, IMediator mediatr)
    {
        this.logger = logger;
        this.mediatr = mediatr;
    }
}