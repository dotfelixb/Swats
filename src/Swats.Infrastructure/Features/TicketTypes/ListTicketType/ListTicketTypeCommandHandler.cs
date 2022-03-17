using FluentResults;
using MediatR;
using Swats.Data.Repository;
using Swats.Model.Domain;
using Swats.Model.Queries;

namespace Swats.Infrastructure.Features.TicketTypes.GetTicketType;

public class ListTicketTypeCommandHandler : IRequestHandler<ListTicketTypeCommand, Result<IEnumerable<TicketType>>>
{   
     private readonly ITicketRepository _ticketRepository;

    public ListTicketTypeCommandHandler(ITicketRepository ticketRepository)
    {
        _ticketRepository = ticketRepository;
    }

    public async Task<Result<IEnumerable<TicketType>>> Handle(ListTicketTypeCommand request, CancellationToken cancellationToken)
    {
        var rst = await _ticketRepository.ListTicketTypes(request.Offset, request.Limit, cancellationToken);

        return Result.Ok(rst);
    }
}