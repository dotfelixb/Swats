using MassTransit;
using System.Text.Json.Serialization;

namespace Keis.Model.Domain;

public class DbAudit
{
    #region UI Props

    public string ImageCode { get; set; }
    public string CreatedByName { get; set; }
    public string UpdatedByName { get; set; }

    #endregion UI Props

    #region Db Props

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public DefaultStatus Status { get; set; }

    public string RowVersion { get; set; } = Guid.NewGuid().ToString();
    public bool Deleted { get; set; }
    public string CreatedBy { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public string UpdatedBy { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    #endregion Db Props
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