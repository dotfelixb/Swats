using FluentResults;
using MediatR;
using Swats.Data.Repository;
using Swats.Model.Commands;
using Swats.Model.Queries;

namespace Swats.Infrastructure.Features.BusinessHour.GetBusinessHour;

public class GetBusinessHourCommandHandler : IRequestHandler<GetBusinessHourCommand, Result<FetchBusinessHour>>
{
    private readonly IManageRepository _manageRepository;

    public GetBusinessHourCommandHandler(IManageRepository manageRepository)
    {
        _manageRepository = manageRepository;
    }

    public async Task<Result<FetchBusinessHour>> Handle(GetBusinessHourCommand request, CancellationToken cancellationToken)
    {
        var rst = await _manageRepository.GetBusinessHour(request.Id, cancellationToken);

        return rst.Id == Guid.Empty
            ? Result.Fail<FetchBusinessHour>($"Business Hour with id [{request.Id}] does not exist!")
            : Result.Ok(rst);
    }
}