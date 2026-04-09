using Domain.Enums;
using Domain.Models.Organization;

namespace Domain.Models.Profile;

public class UserXpLedger
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public int Amount { get; set; }
    public XpReason Reason { get; set; }
    public string? ReferenceType { get; set; }
    public Guid? ReferenceId { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
}
