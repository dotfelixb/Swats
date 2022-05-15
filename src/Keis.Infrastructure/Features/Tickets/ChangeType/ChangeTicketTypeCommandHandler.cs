using System.Text.Json;
using FluentResults;
using Keis.Data.Repository;
using Keis.Model.Domain;
using MediatR;

namespace Keis.Infrastructure.Features.Tickets.ChangeType;

public class ChangeTicketTypeCommandHandler : IRequestHandler<ChangeTicketTypeCommand, Result<string>>
{
      private readonly ITicketRepository _ticketRepository;

    public ChangeTicketTypeCommandHandler(ITicketRepository ticketRepository)
    {
        _ticketRepository = ticketRepository;
    }

    public async Task<Result<string>> Handle(ChangeTicketTypeCommand request, CancellationToken cancellationToken)
    {
         var auditLog = new DbAuditLog
        {
            Target = request.Id,
            ActionName = "ticket.update",
            Description = "change ticket help topic",
            ObjectName = "ticket",
            ObjectData = JsonSerializer.Serialize(request),
            CreatedBy = request.CreatedBy
        };

        var rst = await _ticketRepository.ChangeTicketType(request.Id, request.TicketType, request.CreatedBy, auditLog, cancellationToken);

        return rst > 0
           ? Result.Ok("Ticket Type updated successfully")
           : Result.Fail<string>("Not able to update ticket now!");
    }
}
