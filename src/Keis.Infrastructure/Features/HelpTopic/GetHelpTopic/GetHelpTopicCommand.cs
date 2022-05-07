using FluentResults;
using Keis.Model.Queries;
using MediatR;

namespace Keis.Infrastructure.Features.HelpTopic.GetHelpTopic;

public class GetHelpTopicCommand : GetType, IRequest<Result<FetchHelpTopic>>
{
}