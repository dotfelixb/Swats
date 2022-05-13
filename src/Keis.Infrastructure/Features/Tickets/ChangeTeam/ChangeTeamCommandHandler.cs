using System;
using System.Text.Json;
using FluentResults;
using FluentValidation;
using Keis.Data.Repository;
using Keis.Model.Domain;
using MediatR;

namespace Keis.Infrastructure.Features.Tickets.ChangeTeam;

public class ChangeTeamCommand : IRequest<Result<string>>
{
	public string Id { get; set; }
	public string Team { get; set; }
	public string CreatedBy { get; set; }
}

public class ChangeTeamCommandValidator : AbstractValidator<ChangeTeamCommand>
{
    public ChangeTeamCommandValidator()
    {
		RuleFor(r => r.Id).NotEmpty();
		RuleFor(r => r.Team).NotEmpty();
    }
}

public class ChangeTeamCommandHandler : IRequestHandler<ChangeTeamCommand, Result<string>>
{
	private readonly ITicketRepository _ticketRepository;

    public ChangeTeamCommandHandler(ITicketRepository ticketRepository)
    {
        _ticketRepository = ticketRepository;
    }

    public async Task<Result<string>> Handle(ChangeTeamCommand request, CancellationToken cancellationToken)
    {
        var auditLog = new DbAuditLog
        {
            Target = request.Id,
            ActionName = "ticket.update",
            Description = "change ticket department",
            ObjectName = "ticket",
            ObjectData = JsonSerializer.Serialize(request),
            CreatedBy = request.CreatedBy
        };

        var rst = await _ticketRepository.ChangeTeam(request.Id, request.Team, request.CreatedBy, auditLog, cancellationToken);

        return rst > 0
            ? Result.Ok("Team updated successfully")
            : Result.Fail<string>("Not able to update ticket now!");
    }
}

