using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PasswordManager.Core.DTOs;
using PasswordManager.Core.Interfaces;

[ApiController]
[Route("api/payment")]
public class PaymentGatewayController : ControllerBase
{
    private readonly IPaymentService _paymentServices;
    public PaymentGatewayController(IPaymentService paymentService)
    {
        _paymentServices = paymentService;
    }

    [HttpPost("{gateway}")]
    public async Task<IActionResult> CreatePayment(string gateway, [FromBody] PaymentRequest request)
    {
        return Ok(await _paymentServices.CreatePaymentAsync(gateway, request));
    }
}