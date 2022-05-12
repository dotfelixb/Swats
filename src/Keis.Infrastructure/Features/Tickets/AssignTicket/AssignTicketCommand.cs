using FluentResults;
using MediatR;

namespace Keis.Infrastructure.Features.Tickets.AssignTicket;

public class AssignTicketCommand : IRequest<Result<string>>
{
    public string Id { get; set; }
    public string AssignedTo { get; set; }
    public string CreatedBy { get; set; }
}
