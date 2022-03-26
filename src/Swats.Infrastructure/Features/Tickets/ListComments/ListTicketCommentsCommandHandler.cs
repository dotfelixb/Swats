using FluentResults;
using MediatR;
using Swats.Data.Repository;
using Swats.Model.Commands;
using Swats.Model.Queries;

namespace Swats.Infrastructure.Features.Tickets.ListComments;

public class ListTicketCommentsCommandHandler : IRequestHandler<ListTicketCommentCommand, Result<IEnumerable<FetchTicketComment>>>
{
    private readonly ITicketRepository _ticketRepository;

    public ListTicketCommentsCommandHandler(ITicketRepository ticketRepository)
    {
        _ticketRepository = ticketRepository;
    }

    public async Task<Result<IEnumerable<FetchTicketComment>>> Handle(ListTicketCommentCommand request, CancellationToken cancellationToken)
    {
        var result = await _ticketRepository.ListTicketComments(request.TicketId, cancellationToken);

        return Result.Ok(result);
    }
}