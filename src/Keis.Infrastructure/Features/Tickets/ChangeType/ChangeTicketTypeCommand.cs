using FluentResults;
using MediatR;

namespace Keis.Infrastructure.Features.Tickets.ChangeType;

public class ChangeTicketTypeCommand : IRequest<Result<string>>
{
    public string Id { get; set; }
    public string TicketType { get; set; }
    public string CreatedBy { get; set; }
}
