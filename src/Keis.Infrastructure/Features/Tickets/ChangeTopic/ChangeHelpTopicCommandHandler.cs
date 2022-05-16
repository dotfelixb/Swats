using System.Text.Json;
using FluentResults;
using Keis.Data.Repository;
using Keis.Model.Domain;
using MediatR;

namespace Keis.Infrastructure.Features.Tickets.ChangeTopic;
public class ChangeHelpTopicCommandHandler : IRequestHandler<ChangeHelpTopicCommand, Result<string>>
{
      private readonly ITicketRepository _ticketRepository;

    public ChangeHelpTopicCommandHandler(ITicketRepository ticketRepository)
    {
        _ticketRepository = ticketRepository;
    }

    public async Task<Result<string>> Handle(ChangeHelpTopicCommand request, CancellationToken cancellationToken)
    {
        var auditLog = new DbAuditLog
        {
            Target = request.Id,
            ActionName = "ticket.update",
            Description = "change ticket help topic",
            ObjectName = "ticket",
            ObjectData = JsonSerializer.Serialize(request),
            CreatedBy = request.CreatedBy
        };

        var rst = await _ticketRepository.ChangeHelpTopic(request.Id, request.HelpTopic, request.CreatedBy, auditLog, cancellationToken);

        return rst > 0
           ? Result.Ok("Help Topic updated successfully")
           : Result.Fail<string>("Not able to update ticket now!");
    }
}
