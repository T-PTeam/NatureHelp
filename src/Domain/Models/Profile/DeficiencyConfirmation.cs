using Domain.Enums;
using Domain.Models.Organization;

namespace Domain.Models.Profile;

public class DeficiencyConfirmation
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public Guid DeficiencyId { get; set; }
    public EDeficiencyType DeficiencyType { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
}
