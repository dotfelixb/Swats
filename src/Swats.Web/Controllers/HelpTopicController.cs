using MediatR;

namespace Swats.Web.Controllers;

public class HelpTopicController : MethodController
{
    private readonly ILogger<HelpTopicController> logger;
    private readonly IMediator mediatr;

    public HelpTopicController(ILogger<HelpTopicController> logger, IMediator mediatr)
    {
        this.logger = logger;
        this.mediatr = mediatr;
    }
}