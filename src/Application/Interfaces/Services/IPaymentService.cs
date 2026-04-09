using Application.Dtos;

namespace Application.Interfaces.Services;

public interface IPaymentService
{
    Task<PaymentResponseDto> CreateDonationPaymentAsync(PaymentRequestDto request);
    Task<bool> VerifyPaymentCallbackAsync(string data, string signature);
}
