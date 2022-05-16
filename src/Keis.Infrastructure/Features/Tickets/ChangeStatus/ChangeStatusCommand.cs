using FluentResults;
using Keis.Model.Domain;
using MediatR;

namespace Keis.Infrastructure.Features.Tickets.ChangeStatus;

public class ChangeStatusCommand : IRequest<Result<string>>
{
    public string Id { get; set; }
    public TicketStatus Status { get; set; }
    public string CreatedBy { get; set; }
}

