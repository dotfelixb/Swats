using AutoMapper;
using FluentResults;
using MassTransit;
using MediatR;
using Swats.Data.Repository;
using Swats.Infrastructure.Extensions;
using Swats.Model.Commands;
using Swats.Model.Domain;

namespace Swats.Infrastructure.Features.Tickets.CreateTicket;

public class CreateTicketCommandHandler : IRequestHandler<CreateTicketCommand, Result>
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IMapper _mapper;

    public CreateTicketCommandHandler(ITicketRepository ticketRepository, IMapper mapper)
    {
        _ticketRepository = ticketRepository;
        _mapper = mapper;
    }

    public async Task<Result> Handle(CreateTicketCommand request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var ticket = _mapper.Map<CreateTicketCommand, Ticket>(request);
        ticket.Id = NewId.NextGuid();
        ticket.UpdatedBy = request.CreatedBy;

        var code = await _ticketRepository.GenerateTicketCode(cancellationToken);
        ticket.Code = code.FormatCode();

        var result = await _ticketRepository.CreateTicket(ticket, cancellationToken);

        if (result < 1)
        {
            return Result.Fail("Not able to create ticket");
        }

        return Result.Ok().WithSuccess("Created Ticket");
    }
}