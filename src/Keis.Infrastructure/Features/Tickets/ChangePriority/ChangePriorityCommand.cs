using FluentResults;
using Keis.Model.Domain;
using MediatR;

namespace Keis.Infrastructure.Features.Tickets.ChangePriority;

public class ChangePriorityCommand : IRequest<Result<string>>
{
    public string Id { get; set; }
    public TicketPriority Priority { get; set; }
    public string CreatedBy { get; set; }
}
