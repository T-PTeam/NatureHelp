using System.ComponentModel.DataAnnotations;

namespace Shared.Dtos;
public class UserDto
{
    public string Email { get; set; } = string.Empty;
}

public class SendPasswordResetLinkDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}

public class ResetPasswordDto
{
    [Required]
    public string Token { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]
    public string NewPassword { get; set; } = string.Empty;
}

public class ResetPasswordWithTokenDto
{
    [Required]
    public string ResetPasswordToken { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]
    public string NewPassword { get; set; } = string.Empty;
}
