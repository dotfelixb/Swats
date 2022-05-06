using FluentResults;
using Keis.Model.Domain;
using MediatR;

namespace Keis.Infrastructure.Features.Workflow.ListWorkflowEvent;

public class
    ListWorkflowEventCommandHandler : IRequestHandler<ListWorkflowEventCommand, Result<IEnumerable<WorkflowEvent>>>
{
    public Task<Result<IEnumerable<WorkflowEvent>>> Handle(ListWorkflowEventCommand request,
        CancellationToken cancellationToken)
    {
        // we intentionally return this list
        IEnumerable<WorkflowEvent> events = new List<WorkflowEvent>
        {
            new() {Name = "New Ticket", Type = EventType.NewTicket},
            new() {Name = "Change Ticket Type", Type = EventType.ChangeTicketType},
            new() {Name = "Change Department", Type = EventType.ChangeDepartment},
            new() {Name = "Change Team", Type = EventType.ChangeTeam},
            new() {Name = "Change Ticket Priority", Type = EventType.ChangeTicketPriority},
            new() {Name = "Change Ticket Status", Type = EventType.ChangeTicketStatus}
        };

        return Task.FromResult(Result.Ok(events));
    }
}