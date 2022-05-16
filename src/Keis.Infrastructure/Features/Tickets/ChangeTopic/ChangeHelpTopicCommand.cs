using FluentResults;
using MediatR;

namespace Keis.Infrastructure.Features.Tickets.ChangeTopic;

public class ChangeHelpTopicCommand : IRequest<Result<string>>
{
    public string Id { get; set; }
    public string HelpTopic { get; set; }
    public string CreatedBy { get; set; }
}
