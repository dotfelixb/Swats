using System.Text.Json;
using AutoMapper;
using FluentResults;
using Keis.Data.Repository;
using Keis.Model.Domain;
using MediatR;

namespace Keis.Infrastructure.Features.TicketTypes.CreateTicketType;

public class CreateTicketTypeCommandHandler : IRequestHandler<CreateTicketTypeCommand, Result<string>>
{
    private readonly IMapper _mapper;
    private readonly ITicketRepository _ticketRepository;

    public CreateTicketTypeCommandHandler(ITicketRepository ticketRepository, IMapper mapper)
    {
        _ticketRepository = ticketRepository;
        _mapper = mapper;
    }

    public async Task<Result<string>> Handle(CreateTicketTypeCommand request, CancellationToken cancellationToken)
    {
        var ticketType = _mapper.Map<CreateTicketTypeCommand, TicketType>(request);

        var auditLog = new DbAuditLog
        {
            Target = ticketType.Id,
            ActionName = "tickettype.create",
            Description = "added ticket type",
            ObjectName = "tickettype",
            ObjectData = JsonSerializer.Serialize(ticketType),
            CreatedBy = request.CreatedBy
        };

        var rst = await _ticketRepository.CreateTicketType(ticketType, auditLog, cancellationToken);
        return rst > 0 ? Result.Ok(ticketType.Id) : Result.Fail<string>("Not able to create now!");
    }
}