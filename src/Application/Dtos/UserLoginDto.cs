namespace Application.Dtos;
public class UserLoginDto()
{
    public Guid? UserId { get; set; }
    public Guid? OrganizationId { get; set; }
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string PasswordHash { get; set; } = "";
};