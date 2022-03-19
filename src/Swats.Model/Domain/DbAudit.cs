using MassTransit;

namespace Swats.Model.Domain;

public class DbAudit
{
    #region UI Props
    public string ImageCode { get; set; }
    public string CreatedByName { get; set; }
    public string UpdatedByName { get; set; }
    #endregion

    #region Db Props
    public Guid RowVersion { get; set; } = Guid.NewGuid();
    public bool Deleted { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public Guid UpdatedBy { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    #endregion
}

public class DbAuditLog
{
    public Guid Id { get; set; } = NewId.NextGuid();
    public Guid Target { get; set; }
    public string ActionName { get; set; }
    public string Description { get; set; }
    public string ObjectName { get; set; }
    public string ObjectData { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}
