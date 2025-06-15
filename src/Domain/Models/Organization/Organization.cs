namespace Domain.Models.Organization;

public class Organization : BaseModel
{
    private List<User> staff = new List<User>();


    public string Title { get; set; } = null!;
    public int AllowedMembersCount { get; set; } = 11;

    public void AddMember(User user)
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
