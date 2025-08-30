using Domain.Enums;
using Domain.Models.Organization;

namespace Domain.Models.Audit;
public class DeficiencyMonitoring : BaseModel
{
    public bool IsMonitoring { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public EDeficiencyType DeficiencyType { get; set; }
    public Guid DeficiencyId { get; set; }
}
