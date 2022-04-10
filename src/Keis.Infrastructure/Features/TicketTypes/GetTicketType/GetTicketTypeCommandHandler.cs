using FluentResults;
using MediatR;
using Keis.Data.Repository;
using Keis.Model.Commands;
using Keis.Model.Queries;

namespace Keis.Infrastructure.Features.TicketTypes.GetTicketType;

public class GetTicketTypeCommandHandler : IRequestHandler<GetTicketTypeCommand, Result<FetchTicketType>>
{
    private readonly ITicketRepository _ticketRepository;

    public GetTicketTypeCommandHandler(ITicketRepository ticketRepository)
    {
        _ticketRepository = ticketRepository;
    }

    public async Task<Result<FetchTicketType>> Handle(GetTicketTypeCommand request, CancellationToken cancellationToken)
    {
        var rst = await _ticketRepository.GetTicketType(request.Id, cancellationToken);

        return rst is null
            ? Result.Fail<FetchTicketType>($"Ticket Type with Id {request.Id} does not exist!")
            : Result.Ok(rst);
    }
}