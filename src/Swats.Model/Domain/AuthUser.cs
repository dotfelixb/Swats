using MassTransit;
using Microsoft.AspNetCore.Identity;

namespace Swats.Model.Domain;

public class AuthUser : DbAudit
{
    public Guid Id { get; set; } = NewId.NextGuid();
    public string UserName { get; set; }
    public string NormalizedUserName { get; set; }
    public string Email { get; set; }
    public string NormalizedEmail { get; set; }
    public bool EmailConfirmed { get; set; }
    public string PasswordHash { get; set; }
    public string SecurityStamp { get; set; }
    public string Phone { get; set; }
    public bool PhoneConfirmed { get; set; }
    public bool TwoFactorEnabled { get; set; }
    public bool Lockout { get; set; }
    public int FailedCount { get; set; }
}