using FluentResults;
using MediatR;

namespace Keis.Model.Commands;

#region Ticket


public class TicketCommentCommand
{
    public string CommentId { get; set; }
    public string TicketId { get; set; }
    public string Body { get; set; }
}

public class CreateTicketReplyCommand : TicketCommentCommand, IRequest<Result<string>>
{
    public string Subject { get; set; }
    public string ToEmail { get; set; }
    public string[][] Recipients { get; set; }
}

#endregion