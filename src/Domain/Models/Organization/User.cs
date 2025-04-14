using Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models.Organization;

public class User : Person
{
    private ERole role = ERole.Supervisor;

    public ERole Role { get => role; private set => role = value; }
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public Organization? Organization { get; set; }
    public Laboratory? Laboratory { get; set; }
    public string? AccessToken { get; set; }
    public DateTime? AccessTokenExpireTime { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpireTime { get; set; }

    [ForeignKey(nameof(Organization))]
    public Guid? OrganizationId { get; set; }

    [ForeignKey(nameof(Laboratory))]
    public Guid? LaboratoryId { get; set; }

    [NotMapped]
    public string Password { get; set; } = null!;
    public bool IsEmailValid(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            return false;
        }

        string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

        return System.Text.RegularExpressions.Regex.IsMatch(password, pattern);
    }
    public bool IsPasswordValid(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            return false;
        }

        string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";

        return System.Text.RegularExpressions.Regex.IsMatch(password, pattern);
    }

    public void AssignRole(ERole role)
    {
        this.role = role;
    }

    public bool HasRole(ERole role)
    {
        return this.role == role;
    }
}
