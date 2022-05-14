using System.Text.Json;
using FluentResults;
using Keis.Data.Repository;
using Keis.Model.Domain;
using MediatR;

namespace Keis.Infrastructure.Features.Tickets.ChangeDue;

public class ChangeDueCommandHandler : IRequestHandler<ChangeDueCommand, Result<DateTimeOffset>>
{
    private readonly ITicketRepository _ticketRepository;

    public ChangeDueCommandHandler(ITicketRepository ticketRepository)
    {
        _ticketRepository = ticketRepository;
    }

    public async Task<Result<DateTimeOffset>> Handle(ChangeDueCommand request, CancellationToken cancellationToken)
    {
        var auditLog = new DbAuditLog
        {
            Target = request.Id,
            ActionName = "ticket.update",
            Description = "change ticket due date",
            ObjectName = "ticket",
            ObjectData = JsonSerializer.Serialize(request),
            CreatedBy = request.CreatedBy
        };

        var rst = await _ticketRepository.ChangeDueDate(request.Id, request.DueAt, request.CreatedBy, auditLog, cancellationToken);

        return rst > 0
           ? Result.Ok(request.DueAt)
           : Result.Fail<DateTimeOffset>("Not able to update ticket now!");
    }
}