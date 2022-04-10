namespace Keis.Model.Domain;

public class AuthRole : DbAudit
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string NormalizedName { get; set; }
}