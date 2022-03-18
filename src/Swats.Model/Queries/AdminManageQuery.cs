using FluentResults;
using MediatR;
using Swats.Model.Domain;

namespace Swats.Model.Queries;

internal class AdminManageQuery
{
}

public class GetTicketTypeCommand : IRequest<Result<FetchTicketType>>
{
    public Guid Id { get; set; }
}

public class ListType
{
    public int Offset { get; set; } = 0;
    public int Limit { get; set; } = 20;
    public bool Deleted { get; set; }
}

public class ListTicketTypeCommand : ListType, IRequest<Result<IEnumerable<FetchTicketType>>>
{
}

public class FetchTicketType : TicketType
{
    public string ImageCode { get; set; }
    public string CreatedByName { get; set; }
    public string UpdatedByName { get; set; }
}

public class GetBusinessHourCommand : IRequest<Result<FetchBusinessHour>>
{
    public Guid Id { get; set; }
}
