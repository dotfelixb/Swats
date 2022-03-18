using FluentResults;
using MediatR;
using Swats.Data.Repository;
using Swats.Model.Domain;
using Swats.Model.Queries;

namespace Swats.Infrastructure.Features.TicketTypes.GetTicketType;

internal class GetTicketTypeCommandHandler : IRequestHandler<GetTicketTypeCommand, Result<FetchTicketType>>
{
    private readonly ITicketRepository _ticketRepository;

    public GetTicketTypeCommandHandler(ITicketRepository ticketRepository)
    {
        _ticketRepository = ticketRepository;
    }

    public async Task<Result<FetchTicketType>> Handle(GetTicketTypeCommand request, CancellationToken cancellationToken)
    {
        var rst = await _ticketRepository.GetTicketType(request.Id, cancellationToken);

        return rst.Id == Guid.Empty 
            ? Result.Fail<FetchTicketType>($"Ticket Type with Id {request.Id} does not exist")
            : Result.Ok(rst);
    }
}

