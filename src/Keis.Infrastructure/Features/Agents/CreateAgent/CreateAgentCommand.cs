using FluentResults;
using Keis.Model.Domain;
using MassTransit;
using MediatR;

namespace Keis.Infrastructure.Features.Agents.CreateAgent;

public class CreateAgentCommand : IRequest<Result<string>>
{
    public string Id { get; set; } = NewId.NextGuid().ToString(); // need for agent and user creation
    public string UserName { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Mobile { get; set; }
    public string Telephone { get; set; }
    public string Timezone { get; set; }
    public string Department { get; set; }
    public string Team { get; set; }
    public string TicketType { get; set; }
    public DefaultStatus Status { get; set; }
    public AgentMode Mode { get; set; }
    public string CreatedBy { get; set; }
    public string Note { get; set; }
}