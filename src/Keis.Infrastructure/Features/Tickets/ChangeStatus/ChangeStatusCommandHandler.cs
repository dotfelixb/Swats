using System.Text.Json;
using FluentResults;
using Keis.Data.Repository;
using Keis.Model.Domain;
using MediatR;

namespace Keis.Infrastructure.Features.Tickets.ChangeStatus;

public class ChangeStatusCommandHandler : IRequestHandler<ChangeStatusCommand, Result<string>>
{
    private readonly ITicketRepository _ticketRepository;

    public ChangeStatusCommandHandler(ITicketRepository ticketRepository)
    {
        _ticketRepository = ticketRepository;
    }

    public async Task<Result<string>> Handle(ChangeStatusCommand request, CancellationToken cancellationToken)
    {
        var auditLog = new DbAuditLog
        {
            Target = request.Id,
            ActionName = "ticket.update",
            Description = "change ticket status",
            ObjectName = "ticket",
            ObjectData = JsonSerializer.Serialize(request),
            CreatedBy = request.CreatedBy
        };

        var rst = await _ticketRepository.ChangeStatus(request.Id, request.Status, request.CreatedBy, auditLog, cancellationToken);

        return rst > 0
            ? Result.Ok(request.Status.ToText())
            : Result.Fail<string>("Not able to update ticket now!");
    }
}

