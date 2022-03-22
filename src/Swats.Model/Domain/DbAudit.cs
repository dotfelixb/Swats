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

    public DefaultStatus Status { get; set; }
    public string RowVersion { get; set; } = Guid.NewGuid().ToString();
    public bool Deleted { get; set; }
    public string CreatedBy { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public string UpdatedBy { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    #endregion
}

public class DbAuditLog
{
    public string Id { get; set; } = NewId.NextGuid().ToString();
    public string Target { get; set; }
    public string ActionName { get; set; }
    public string Description { get; set; }
    public string ObjectName { get; set; }
    public string ObjectData { get; set; }
    public string CreatedBy { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}