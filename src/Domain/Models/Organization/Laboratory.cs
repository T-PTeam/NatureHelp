using Domain.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models.Organization;

public class Laboratory : BaseModel, ICoordinates
{
    public string Title { get; set; } = null!;

    [NotMapped]
    public int ResearchersCount { get; set; }

    public List<User>? Researchers { get; set; }

    #region ICoordinates Implementation
    public double Longitude { get; set; }
    public double Latitude { get; set; }
    public string? Address { get; set; }
    #endregion
}
