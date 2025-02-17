namespace Application.Dtos;
public class UserLoginDto()
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string PasswordHash { get; set; } = "";
};