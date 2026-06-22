namespace Domain.Models.Organization;

public class UserLaboratory
{
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public Guid LaboratoryId { get; set; }
    public Laboratory Laboratory { get; set; } = null!;
}
