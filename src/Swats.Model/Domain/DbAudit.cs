namespace Swats.Model.Domain;

public class DbAudit
{
    public bool Deleted { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}

public class DbAuditLog
{
    public Guid Id { get; set; }
    public Guid TargetId { get; set; }
    public string ActionName { get; set; }
    public string ObjectName { get; set; }
    public string ObjectData { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}