using FluentResults;
using Keis.Model;
using MediatR;

namespace Keis.Infrastructure.Features.Tickets.AssignTicket;

public class AssignTicketCommand : IRequest<Result<SingleResult<string>>>
{
    public string Id { get; set; }
    public string AssignedTo { get; set; }
    public string CreatedBy { get; set; }
}

public class AssignTicketCommandHandler
{
}