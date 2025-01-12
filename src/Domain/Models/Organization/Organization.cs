namespace Domain.Models.Organization;

public class Organization : BaseEntity
{
    private List<User> staff = new List<User>();


    public string Title { get; set; } = null!;
    public Location Location { get; set; } = null!;

    void AddMember(User user)
    {
        staff.Add(user);
    }

    public void RemoveMember(User user)
    {
        staff.Remove(user);
    }

    public List<User> GetMembers()
    {
        return staff.ToList();
    }

    public bool IsMember(User user)
    {
        return staff.Contains(user);
    }
}
