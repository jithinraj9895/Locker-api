using PasswordManager.Core.DTOs;
using PasswordManager.Core.Interfaces;

public class PaymentServices : IPaymentService
{
    private readonly IPaymentGatewayFactory _paymentFactory;

    public PaymentServices(IPaymentGatewayFactory paymentGatewayFactory)
    {
        _paymentFactory = paymentGatewayFactory;
    }
    public async Task<PaymentResult> CreatePaymentAsync(string gateway, PaymentRequest request)
    {
        return await _paymentFactory.GetGateway(gateway).CreatePaymentAsync(request);
    }
}