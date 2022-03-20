using MassTransit;

namespace Swats.Model.Domain;

public class HelpTopic : DbAudit
{
    public string Id { get; set; } = NewId.NextGuid().ToString();
    public string Topic { get; set; }
    public DefaultType Type { get; set; }
    public string Department { get; set; }
    public string DefaultDepartment { get; set; }
    public string Note { get; set; }
}

public class HelpTopicAuditLog : DbAuditLog
{
}