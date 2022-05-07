using FluentResults;
using Keis.Model.Queries;
using MediatR;

namespace Keis.Infrastructure.Features.HelpTopic.ListHelpTopics;

public class ListHelpTopicsCommand : ListType, IRequest<Result<IEnumerable<FetchHelpTopic>>>
{
}