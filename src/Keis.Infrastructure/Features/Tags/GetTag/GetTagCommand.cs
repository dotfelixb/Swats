using FluentResults;
using Keis.Model.Queries;
using MediatR;

namespace Keis.Infrastructure.Features.Tags.GetTag;

public class GetTagCommand : GetType, IRequest<Result<FetchTag>>
{
}