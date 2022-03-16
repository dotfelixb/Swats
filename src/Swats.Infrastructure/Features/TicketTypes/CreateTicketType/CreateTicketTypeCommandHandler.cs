using AutoMapper;
using FluentResults;
using MassTransit;
using MediatR;
using Swats.Data.Repository;
using Swats.Model.Commands;
using Swats.Model.Domain;
using System.Text.Json;

namespace Swats.Infrastructure.Features.TicketTypes.CreateTicketType;

public class CreateTicketTypeCommandHandler : IRequestHandler<CreateTicketTypeCommand, Result<Guid>>
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IMapper _mapper;

    public CreateTicketTypeCommandHandler(ITicketRepository ticketRepository, IMapper mapper)
    {
        _ticketRepository = ticketRepository;
        _mapper = mapper;
    }

    public async Task<Result<Guid>> Handle(CreateTicketTypeCommand request, CancellationToken cancellationToken)
    {
        var ticketType = _mapper.Map<CreateTicketTypeCommand, TicketType>(request);
       //TODO : use auto mapper
        ticketType.UpdatedBy = ticketType.CreatedBy;

        var auditLog = new DbAuditLog
        {
            Target = ticketType.Id,
            ActionName = "tickettype.create",
            Description = "added ticket type",
            ObjectName = "tickettype",
            ObjectData = JsonSerializer.Serialize(ticketType),
            CreatedBy = request.CreatedBy,
        };

        var rst= await _ticketRepository.CreateTicketType(ticketType, auditLog, cancellationToken);
        return rst > 0 ? Result.Ok(ticketType.Id) : Result.Fail<Guid>("Not able to crea");
    }
}

