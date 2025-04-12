using Domain.Enums;

namespace Domain.Models.Audit;
public class ChangedModelLog : BaseModel
{
    public EDeficiencyType DeficiencyType { get; set; }
    public Guid DeficiencyId { get; set; }
    public string ChangesJson { get; set; }
}
