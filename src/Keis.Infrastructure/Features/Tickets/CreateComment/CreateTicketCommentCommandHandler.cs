using FluentResults;
using Keis.Data.Repository;
using Keis.Model.Domain;
using MediatR;

namespace Keis.Infrastructure.Features.Tickets.CreateComment;

public class CreateTicketCommentCommandHandler : IRequestHandler<CreateTicketCommentCommand, Result<string>>
{
    private readonly ITicketRepository _ticketRepository;

    public CreateTicketCommentCommandHandler(ITicketRepository ticketRepository)
    {
        _ticketRepository = ticketRepository;
    }

    public async Task<Result<string>> Handle(CreateTicketCommentCommand request, CancellationToken cancellationToken)
    {
        var comment = new TicketComment
        {
            Ticket = request.TicketId,
            FromEmail = request.FromEmail,
            FromName = request.FromName,
            Body = request.Body,
            Type = CommentType.Comment,
            Source = TicketSource.App, // TODO
            Target = request.CommentId,
            CreatedBy = request.CreatedBy
        };

        var result = await _ticketRepository.CreateTicketComment(comment, cancellationToken);
        return result > 0 ? Result.Ok(comment.Id) : Result.Fail("Not able to create now");
    }
}