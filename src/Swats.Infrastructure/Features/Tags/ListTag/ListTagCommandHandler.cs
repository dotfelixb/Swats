using FluentResults;
using MediatR;
using Swats.Data.Repository;
using Swats.Model.Commands;
using Swats.Model.Queries;

namespace Swats.Infrastructure.Features.Tags.ListTag;

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