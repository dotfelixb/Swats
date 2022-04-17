using FluentResults;
using MediatR;
using Keis.Data.Repository;
using Keis.Model.Commands;
using Keis.Model.Queries;

namespace Keis.Infrastructure.Features.Tags.ListTag;

public class ListTagsCommand : ListType, IRequest<Result<IEnumerable<FetchTag>>>
{
}

public class ListTagCommandHandler : IRequestHandler<ListTagsCommand, Result<IEnumerable<FetchTag>>>
{
    private readonly IManageRepository _manageRepository;

    public ListTagCommandHandler(IManageRepository manageRepository)
    {
        _manageRepository = manageRepository;
    }

    public async Task<Result<IEnumerable<FetchTag>>> Handle(ListTagsCommand request,
        CancellationToken cancellationToken)
    {
        var rst = await _manageRepository.ListTags(request.Offset, request.Limit, request.Deleted, cancellationToken);

        return Result.Ok(rst);
    }
}