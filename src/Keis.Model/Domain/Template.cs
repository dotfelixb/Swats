using MassTransit;

namespace Keis.Model.Domain;

public class Template : DbAudit
{
    public string Id { get; set; } = NewId.NextGuid().ToString();
    public string Name { get; set; }
    public string[] MergeTags { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
}