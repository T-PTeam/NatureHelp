using Application.Dtos;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NatureHelp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;
    private readonly ILogger<PaymentController> _logger;

    public PaymentController(IPaymentService paymentService, ILogger<PaymentController> logger)
    {
        _paymentService = paymentService;
        _logger = logger;
    }

    /// <summary>
    /// Create a donation payment
    /// </summary>
    /// <param name="request">Payment request details</param>
    /// <returns>Payment response with data and signature</returns>
    [HttpPost("create-donation")]
    [AllowAnonymous]
    public async Task<IActionResult> CreateDonationPayment([FromBody] PaymentRequestDto request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _paymentService.CreateDonationPaymentAsync(request);
            
            _logger.LogInformation("Payment created for amount: {Amount} {Currency}", request.Amount, request.Currency);
            
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating donation payment");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Handle payment callback from payment provider
    /// </summary>
    /// <param name="data">Payment data</param>
    /// <param name="signature">Payment signature</param>
    /// <returns>Payment verification result</returns>
    [HttpPost("callback")]
    [AllowAnonymous]
    public async Task<IActionResult> HandlePaymentCallback([FromQuery] string data, [FromQuery] string signature)
    {
        try
        {
            if (string.IsNullOrEmpty(data) || string.IsNullOrEmpty(signature))
            {
                return BadRequest(new { message = "Data and signature are required" });
            }

            var isValid = await _paymentService.VerifyPaymentCallbackAsync(data, signature);
            
            if (isValid)
            {
                _logger.LogInformation("Payment callback verified successfully");
                
                // Here you can add logic to process the successful payment
                // For example, send confirmation email, update database, etc.
                
                return Ok(new { message = "Payment processed successfully" });
            }
            else
            {
                _logger.LogWarning("Invalid payment callback signature");
                return BadRequest(new { message = "Invalid payment signature" });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing payment callback");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Get payment configuration for frontend
    /// </summary>
    /// <returns>Payment configuration</returns>
    [HttpGet("config")]
    [AllowAnonymous]
    public IActionResult GetPaymentConfig()
    {
        try
        {
            var config = new
            {
                PublicKey = _paymentService.GetType().GetProperty("_publicKey")?.GetValue(_paymentService)?.ToString() ?? "",
                Currency = "UAH",
                DefaultAmount = 100,
                Description = "NatureHelp donation"
            };

            return Ok(config);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting payment configuration");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }
}
