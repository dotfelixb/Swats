using System;
using System.Text.Json;
using FluentResults;
using Keis.Data.Repository;
using Keis.Model.Domain;
using MediatR;

namespace Keis.Infrastructure.Features.Tickets.ChangeDepartment;


public class ChangeDepartmentCommandHandler : IRequestHandler<ChangeDepartmentCommand, Result<string>>
{
    private readonly ITicketRepository _ticketRepository;

    public ChangeDepartmentCommandHandler(ITicketRepository ticketRepository)
    {
        _ticketRepository = ticketRepository;
    }

    public async Task<Result<string>> Handle(ChangeDepartmentCommand request, CancellationToken cancellationToken)
    {
        var auditLog = new DbAuditLog
        {
            Target = request.Id,
            ActionName = "ticket.update",
            Description = "change ticket department",
            ObjectName = "ticket",
            ObjectData = JsonSerializer.Serialize(request),
            CreatedBy = request.CreatedBy
        };

        var rst = await _ticketRepository.ChangeDepartment(request.Id, request.Department, request.CreatedBy, auditLog, cancellationToken);

        return rst > 0
           ? Result.Ok("Department updated successfully")
           : Result.Fail<string>("Not able to update ticket now!");
    }
}

