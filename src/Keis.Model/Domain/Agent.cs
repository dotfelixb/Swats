using MassTransit;
using System.Text.Json.Serialization;

namespace Keis.Model.Domain;

public class Agent : DbAudit
{
    public string Id { get; set; } = NewId.NextGuid().ToString();
    public string Name { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Mobile { get; set; }
    public string Telephone { get; set; }
    public string Timezone { get; set; }
    public string Department { get; set; }
    public string Team { get; set; }
    public string Type { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public AgentMode Mode { get; set; }

    public string Note { get; set; }
}