using FluentResults;
using MediatR;
using Swats.Data.Repository;
using Swats.Model.Commands;
using Swats.Model.Queries;

namespace Swats.Infrastructure.Features.BusinessHour.ListBusinessHour;

public class
    ListBusinessHourCommandHandler : IRequestHandler<ListBusinessHourCommand, Result<IEnumerable<FetchBusinessHour>>>
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