using FluentResults;
using FluentValidation;
using Keis.Model.Commands;
using MediatR;

namespace Keis.Infrastructure.Features.Tickets.CreateComment;

public class CreateTicketCommentCommand : TicketCommentCommand, IRequest<Result<string>>
{
    public string FromEmail { get; set; }
    public string FromName { get; set; }
    public string CreatedBy { get; set; }
}
