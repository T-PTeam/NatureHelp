using Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models.Nature;
public class DeficiencyAttachment : Attachment
{
    [ForeignKey(nameof(Deficiency))]
    public Guid DeficiencyId { get; set; }

    [NotMapped]
    public Deficiency Deficiency { get; set; } = null!;

    public EDeficiencyType DeficiencyType { get; set; }
}