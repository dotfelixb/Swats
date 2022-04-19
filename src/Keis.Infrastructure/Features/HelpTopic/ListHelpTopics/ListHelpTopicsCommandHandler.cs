using FluentResults;
using Keis.Data.Repository;
using Keis.Model.Queries;
using MediatR;

namespace Keis.Infrastructure.Features.HelpTopic.ListHelpTopics;

public class ListHelpTopicsCommandHandler : IRequestHandler<ListHelpTopicsCommand, Result<IEnumerable<FetchHelpTopic>>>
{
    private readonly IManageRepository _manageRepository;

    public ListHelpTopicsCommandHandler(IManageRepository manageRepository)
    {
        _manageRepository = manageRepository;
    }

    public async Task<Result<IEnumerable<FetchHelpTopic>>> Handle(ListHelpTopicsCommand request,
        CancellationToken cancellationToken)
    {
        var rst = await _manageRepository.ListHelpTopics(request.Offset, request.Limit, request.Deleted,
            cancellationToken);

        return Result.Ok(rst);
    }
}