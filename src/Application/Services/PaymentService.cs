using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Application.Dtos;
using Application.Interfaces.Services;
using Microsoft.Extensions.Configuration;

namespace Application.Services;

public class PaymentService : IPaymentService
{
    private readonly IConfiguration _configuration;
    private readonly string _publicKey;
    private readonly string _privateKey;
    private readonly string _paymentUrl;

    public PaymentService(IConfiguration configuration)
    {
        _configuration = configuration;
        _publicKey = _configuration["Payment:PublicKey"] ?? throw new InvalidOperationException("Payment:PublicKey not configured");
        _privateKey = _configuration["Payment:PrivateKey"] ?? throw new InvalidOperationException("Payment:PrivateKey not configured");
        _paymentUrl = _configuration["Payment:Url"] ?? throw new InvalidOperationException("Payment:Url not configured");
    }

    public async Task<PaymentResponseDto> CreateDonationPaymentAsync(PaymentRequestDto request)
    {
        var resultUrl = request.ResultUrl ?? _configuration["Payment:ResultUrl"] ?? "https://naturehelp.online/thanks";
        
        var paramsObj = new PaymentParamsDto
        {
            PublicKey = _publicKey,
            Version = 3,
            Action = "pay",
            Amount = request.Amount,
            Currency = request.Currency,
            Description = request.Description,
            ResultUrl = resultUrl
        };

        var json = JsonSerializer.Serialize(paramsObj);
        var data = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));

        var signString = _privateKey + data + _privateKey;
        var sha1 = SHA1.Create();
        var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(signString));
        var signature = Convert.ToBase64String(hash);

        return new PaymentResponseDto
        {
            Data = data,
            Signature = signature,
            PaymentUrl = _paymentUrl
        };
    }

    public async Task<bool> VerifyPaymentCallbackAsync(string data, string signature)
    {
        try
        {
            var signString = _privateKey + data + _privateKey;
            var sha1 = SHA1.Create();
            var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(signString));
            var expectedSignature = Convert.ToBase64String(hash);

            return signature == expectedSignature;
        }
        catch
        {
            return false;
        }
    }
}
