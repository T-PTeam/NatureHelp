namespace Domain.Models;
public class BaseEntity
{
    public Guid Id { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }
}
