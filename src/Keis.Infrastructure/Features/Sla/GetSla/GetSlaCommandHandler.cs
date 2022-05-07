using FluentResults;
using Keis.Data.Repository;
using Keis.Model.Queries;
using MediatR;

namespace Keis.Infrastructure.Features.Sla.GetSla;

public class GetSlaCommandHandler : IRequestHandler<GetSlaCommand, Result<FetchSla>>
{
    private readonly IManageRepository _manageRepository;

    public GetSlaCommandHandler(IManageRepository manageRepository)
    {
        _manageRepository = manageRepository;
    }

    public async Task<Result<FetchSla>> Handle(GetSlaCommand request, CancellationToken cancellationToken)
    {
        var rst = await _manageRepository.GetSla(request.Id, cancellationToken);

        return rst is null
            ? Result.Fail<FetchSla>($"SLA with id [{request.Id}] does not exist!")
            : Result.Ok(rst);
    }
}