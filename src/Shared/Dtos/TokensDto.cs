namespace Shared.Dtos;
public class TokensDto
{
    /// <summary>
    /// Unknown type of token
    /// </summary>
    public string? Token { get; set; }
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
}
