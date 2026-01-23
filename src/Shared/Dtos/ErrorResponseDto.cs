namespace Shared.Dtos;

/// <summary>
/// Standardized error response format for consistent error handling across the API
/// </summary>
public class ErrorResponseDto
{
    public string Message { get; set; } = string.Empty;
    public int StatusCode { get; set; }
    public string? ErrorType { get; set; }
}

