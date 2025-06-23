using Domain.Enums;
using Domain.Interfaces;
using Domain.Models.Nature;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models;
public class CommentMessage : BaseModel, IDeficiencyBindModel
{
    public string Message { get; set; }
    public EDeficiencyType DeficiencyType { get; set; }
    public Guid DeficiencyId { get; set; }
    public string CreatorFullName { get; set; }

    [NotMapped]
    public Deficiency? Deficiency { get; set; }
}
