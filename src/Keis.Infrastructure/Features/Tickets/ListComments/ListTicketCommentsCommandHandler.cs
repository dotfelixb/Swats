using FluentResults;
using MediatR;
using Keis.Data.Repository;
using Keis.Model.Commands;
using Keis.Model.Queries;

namespace Keis.Infrastructure.Features.Tickets.ListComments;

public class ListTicketCommentCommand : ListType, IRequest<Result<IEnumerable<FetchTicketComment>>>
{
    public string TicketId { get; set; }
}

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