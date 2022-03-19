using FluentResults;
using MediatR;
using Swats.Model.Domain;
using Swats.Model.Queries;

namespace Swats.Model.Commands;

public class CreateBusinessHourCommand : IRequest<Result<Guid>>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Timezone { get; set; }
    public DefaultStatus Status { get; set; }
    public Guid CreatedBy { get; set; }
}

public class GetBusinessHourCommand : IRequest<Result<FetchBusinessHour>>
{
    public Guid Id { get; set; }
}

public class ListBusinessHourCommand : ListType, IRequest<Result<IEnumerable<FetchBusinessHour>>>
{
}