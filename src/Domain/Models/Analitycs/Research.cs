using Domain.Enums;
using Domain.Models.Organization;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models.Analitycs;
public class Research : BaseModel
{
    public string Title { get; set; } = null!;
    public ResearchType Type { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; } = null!;

    [ForeignKey(nameof(Laboratory))]
    public Guid LaboratoryId { get; set; }
    public Laboratory? Laboratory { get; set; }

    [ForeignKey(nameof(Researcher))]
    public Guid ResearcherId { get; set; }
    public User? Researcher { get; set; }
}