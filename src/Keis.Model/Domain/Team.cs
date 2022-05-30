using MassTransit;

namespace Keis.Model.Domain;

public class Team : DbAudit
{
    public string Id { get; set; } = NewId.NextGuid().ToString();
    public string Name { get; set; }
    public string Department { get; set; }
    public string Manager { get; set; }
    public string Response { get; set; }
}