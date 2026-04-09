using Domain.Models.Organization;

namespace Domain.Models.Profile;

public class UserDailyVisit
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public DateOnly VisitDate { get; set; }
}
