namespace Domain.Models;
public class BaseModel
{
    public Guid Id { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
}
