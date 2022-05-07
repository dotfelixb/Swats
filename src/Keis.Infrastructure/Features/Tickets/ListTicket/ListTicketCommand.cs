using FluentResults;
using Keis.Model;
using Keis.Model.Queries;
using MediatR;

namespace Keis.Infrastructure.Features.Tickets.ListTicket;

public class ListTicketCommand : ListType, IRequest<Result<IEnumerable<FetchTicket>>>
{
    public string Id { get; set; }
    public bool IncludeDepartment { get; set; }
    public bool IncludeTeam { get; set; }
    public bool IncludeHelpTopic { get; set; }
}