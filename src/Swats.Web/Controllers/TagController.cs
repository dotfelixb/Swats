using MediatR;

namespace Swats.Web.Controllers;

public class TagController : MethodController
{
    private readonly ILogger<TagController> logger;
    private readonly IMediator mediatr;

    public TagController(ILogger<TagController> logger, IMediator mediatr)
    {
        this.logger = logger;
        this.mediatr = mediatr;
    }
}