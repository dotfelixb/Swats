using FluentResults;
using Keis.Data.Repository;
using Keis.Model.Queries;
using MediatR;

namespace Keis.Infrastructure.Features.BusinessHour.ListBusinessHour;

public class ListBusinessHourCommandHandler : IRequestHandler<ListBusinessHourCommand, Result<IEnumerable<FetchBusinessHour>>>
{
    private readonly IManageRepository _manageRepository;

    public ListBusinessHourCommandHandler(IManageRepository manageRepository)
    {
        _manageRepository = manageRepository;
    }

    public async Task<Result<IEnumerable<FetchBusinessHour>>> Handle(ListBusinessHourCommand request,
        CancellationToken cancellationToken)
    {
        var rst = await _manageRepository.ListBusinessHours(request.Offset, request.Limit, request.Deleted,
            cancellationToken);

        return Result.Ok(rst);
    }
}