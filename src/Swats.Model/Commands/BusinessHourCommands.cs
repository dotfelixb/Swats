using FluentResults;
using MediatR;
using Swats.Model.Domain;
using Swats.Model.Queries;

namespace Swats.Model.Commands;

public class CreateBusinessHourCommand : IRequest<Result<string>>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Timezone { get; set; }
    public DefaultStatus Status { get; set; }
    public string CreatedBy { get; set; }
}

public class GetBusinessHourCommand : IRequest<Result<FetchBusinessHour>>
{
    public string Id { get; set; }
}

public class ListBusinessHourCommand : ListType, IRequest<Result<IEnumerable<FetchBusinessHour>>>
{
}