using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models.Organization;

public class Laboratory : BaseModel
{
    public string Title { get; set; } = null!;

    [ForeignKey(nameof(Location))]
    public Guid LocationId { get; set; }
    [NotMapped]
    public int ResearchersCount { get; set; }

    public List<User>? Researchers { get; set; }
    public Location? Location { get; set; }
}
