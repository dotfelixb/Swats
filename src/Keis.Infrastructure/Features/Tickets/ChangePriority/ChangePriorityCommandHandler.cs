using System.Data;
using System.Text.Json;
using FluentResults;
using Keis.Data.Repository;
using Keis.Model.Domain;
using MediatR;

namespace Keis.Infrastructure.Features.Tickets.ChangePriority;

public class ChangePriorityCommandHandler : IRequestHandler<ChangePriorityCommand, Result<string>>
{
     private readonly ITicketRepository _ticketRepository;

    public ChangePriorityCommandHandler(ITicketRepository ticketRepository)
    {
        _ticketRepository = ticketRepository;
    }

    public async Task<Result<string>> Handle(ChangePriorityCommand request, CancellationToken cancellationToken)
    {
        var auditLog = new DbAuditLog
        {
            Target = request.Id,
            ActionName = "ticket.update",
            Description = "change ticket priority",
            ObjectName = "ticket",
            ObjectData = JsonSerializer.Serialize(request),
            CreatedBy = request.CreatedBy
        };

        var rst = await _ticketRepository.ChangePriority(request.Id, request.Priority, request.CreatedBy, auditLog, cancellationToken);

        return rst > 0
            ? Result.Ok(request.Priority.ToText())
            : Result.Fail<string>("Not able to update ticket now!");
    }
}
