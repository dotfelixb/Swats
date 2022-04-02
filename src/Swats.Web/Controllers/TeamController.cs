using MediatR;

namespace Swats.Web.Controllers;

public class TeamController : MethodController
{
    private readonly ILogger<TeamController> logger;
    private readonly IMediator mediatr;

    public TeamController(ILogger<TeamController> logger, IMediator mediatr)
    {
        this.logger = logger;
        this.mediatr = mediatr;
    }
}