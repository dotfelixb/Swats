using System.Text.Json;
using FluentResults;
using Keis.Data.Repository;
using Keis.Model.Domain;
using MediatR;

namespace Keis.Infrastructure.Features.Tickets.AssignTicket;

public class AssignTicketCommandHandler : IRequestHandler<AssignTicketCommand, Result<string>>
{
    private readonly ITicketRepository _ticketRepository;

    public AssignTicketCommandHandler(ITicketRepository ticketRepository)
    {
        _ticketRepository = ticketRepository;
    }

    public async Task<Result<string>> Handle(AssignTicketCommand request, CancellationToken cancellationToken)
    {
        var auditLog = new DbAuditLog
        {
            Target = request.Id,
            ActionName = "ticket.update",
            Description = "assign ticket to",
            ObjectName = "ticket",
            ObjectData = JsonSerializer.Serialize(request),
            CreatedBy = request.CreatedBy
        };

        var rst = await _ticketRepository.AssignTo(request.Id, request.AssignedTo, auditLog, cancellationToken);

        return rst > 0
            ? Result.Ok("Assigned to updated successfully")
            : Result.Fail<string>("Not able to update ticket now!");
    }
}