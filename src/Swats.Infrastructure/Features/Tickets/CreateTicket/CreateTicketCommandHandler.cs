using System.Text.Json;
using AutoMapper;
using FluentResults;
using MediatR;
using Swats.Data.Repository;
using Swats.Infrastructure.Extensions;
using Swats.Model.Commands;
using Swats.Model.Domain;

namespace Swats.Infrastructure.Features.Tickets.CreateTicket;

public class CreateTicketCommandHandler : IRequestHandler<CreateTicketCommand, Result<string>>
{
    private readonly IMapper _mapper;
    private readonly ITicketRepository _ticketRepository;

    public CreateTicketCommandHandler(ITicketRepository ticketRepository, IMapper mapper)
    {
        _ticketRepository = ticketRepository;
        _mapper = mapper;
    }

    public async Task<Result<string>> Handle(CreateTicketCommand request, CancellationToken cancellationToken)
    {
        var ticket = _mapper.Map<CreateTicketCommand, Ticket>(request);
        ticket.UpdatedBy = request.CreatedBy;

        var code = await _ticketRepository.GenerateTicketCode(cancellationToken);
        ticket.Code = code.FormatCode("#PT-"); // Get Prefix from appsettings

        var auditLog = new DbAuditLog
        {
            Target = ticket.Id,
            ActionName = "ticket.create",
            Description = "added ticket",
            ObjectName = "ticket",
            ObjectData = JsonSerializer.Serialize(ticket),
            CreatedBy = request.CreatedBy
        };

        var rst = await _ticketRepository.CreateTicket(ticket, auditLog, cancellationToken);
        return rst > 0 ? Result.Ok(ticket.Id) : Result.Fail<string>("Not able to create now!");
    }
}