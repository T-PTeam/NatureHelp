using System.ComponentModel.DataAnnotations;

namespace Application.Dtos;

public class PaymentRequestDto
{
    [Required]
    public decimal Amount { get; set; }
    
    [Required]
    public string Currency { get; set; } = "UAH";
    
    [Required]
    public string Description { get; set; } = "NatureHelp donation";
    
    public string? ResultUrl { get; set; }
    
    public string? UserEmail { get; set; }
}

public class PaymentResponseDto
{
    public string Data { get; set; } = string.Empty;
    public string Signature { get; set; } = string.Empty;
    public string PaymentUrl { get; set; } = string.Empty;
}

public class PaymentParamsDto
{
    public string PublicKey { get; set; } = string.Empty;
    public int Version { get; set; } = 3;
    public string Action { get; set; } = "pay";
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "UAH";
    public string Description { get; set; } = "NatureHelp donation";
    public string ResultUrl { get; set; } = string.Empty;
}
