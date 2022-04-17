﻿using FluentResults;
using MediatR;
using Keis.Data.Repository;
using Keis.Model.Commands;
using Keis.Model.Queries;

namespace Keis.Infrastructure.Features.Tickets.GetTicket;

public class GetTicketCommand : GetType, IRequest<Result<FetchTicket>>
{
}

public class GetTicketCommandHandler : IRequestHandler<GetTicketCommand, Result<FetchTicket>>
{
    private readonly ITicketRepository _ticketRepository;

    public GetTicketCommandHandler(ITicketRepository ticketRepository)
    {
        _ticketRepository = ticketRepository;
    }

    public async Task<Result<FetchTicket>> Handle(GetTicketCommand request, CancellationToken cancellationToken)
    {
        var rst = await _ticketRepository.GetTicket(request.Id, cancellationToken);

        return rst is null
            ? Result.Fail<FetchTicket>($"Ticket with id [{request.Id}] does not exist!")
            : Result.Ok(rst);
    }
}