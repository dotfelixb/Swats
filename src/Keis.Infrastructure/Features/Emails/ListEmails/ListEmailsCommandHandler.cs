using FluentResults;
using Keis.Data.Repository;
using Keis.Model.Queries;
using MediatR;

namespace Keis.Infrastructure.Features.Emails.ListEmails;

public class ListEmailsCommand : ListType, IRequest<Result<IEnumerable<FetchEmail>>>
{
}

public class ListEmailsCommandHandler : IRequestHandler<ListEmailsCommand, Result<IEnumerable<FetchEmail>>>
{
    private readonly IManageRepository _manageRepository;

    public ListEmailsCommandHandler(IManageRepository manageRepository)
    {
        _manageRepository = manageRepository;
    }

    public async Task<Result<IEnumerable<FetchEmail>>> Handle(ListEmailsCommand request, CancellationToken cancellationToken)
    {
        var rst = await _manageRepository.ListEmails(request.Offset, request.Limit, request.Deleted, cancellationToken);

        return Result.Ok(rst);
    }
}
