using FluentResults;
using Keis.Model.Queries;
using MediatR;

namespace Keis.Infrastructure.Features.Tags.ListTag;

public class ListTagsCommand : ListType, IRequest<Result<IEnumerable<FetchTag>>>
{
}