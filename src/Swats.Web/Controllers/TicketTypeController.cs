using MediatR;

namespace Swats.Web.Controllers;

public class TicketTypeController : MethodController
{
    private readonly ILogger<TicketTypeController> logger;
    private readonly IMediator mediatr;

    public TicketTypeController(ILogger<TicketTypeController> logger, IMediator mediatr)
    {
        this.logger = logger;
        this.mediatr = mediatr;
    }
}