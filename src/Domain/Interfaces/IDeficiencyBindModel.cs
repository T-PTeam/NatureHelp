using Domain.Enums;
using Domain.Models.Nature;

namespace Domain.Interfaces;
public interface IDeficiencyBindModel
{
    public EDeficiencyType DeficiencyType { get; set; }
    public Guid DeficiencyId { get; set; }
    public Deficiency Deficiency { get; set; }
}
