using FluentResults;
using MediatR;
using Swats.Model.Domain;

namespace Swats.Model.Queries;

internal class AdminManageQuery
{
}

public class GetTicketTypeCommand : IRequest<Result<TicketType>>
{
    public Guid Id { get; set; }
}

public class ListTicketTypeCommand : IRequest<Result<IEnumerable<FetchTicketType>>>
{
    public int Offset { get; set; } = 0;
    public int Limit { get; set; } = 20;
    public bool Deleted { get; set; }
}

public class FetchTicketType : TicketType
{
    public string CreatedByName { get; set; }
    public string UpdatedByName { get; set; }
}