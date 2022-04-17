using AutoMapper;
using FluentResults;
using MediatR;
using Keis.Data.Repository;
using Keis.Model.Commands;
using Keis.Model.Domain;
using System.Text.Json;

namespace Keis.Infrastructure.Features.TicketTypes.CreateTicketType;

public class CreateTicketTypeCommand : IRequest<Result<string>>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Color { get; set; }
    public DefaultType Visibility { get; set; }
    public DefaultStatus Status { get; set; }
    public string CreatedBy { get; set; }
}

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