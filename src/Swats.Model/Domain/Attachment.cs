using MassTransit;

namespace Swats.Model.Domain;

public class Attachment : DbAudit
{
    public Guid Id { get; set; } = NewId.NextGuid();
    public Guid Target { get; set; }
    public AttachmentSource Source { get; set; }
    public string FilePath { get; set; }
    public string AbsolutePath { get; set; }
}
