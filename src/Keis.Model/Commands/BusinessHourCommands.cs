using FluentResults;
using MediatR;
using Keis.Model.Domain;
using Keis.Model.Queries;

namespace Keis.Model.Commands;

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