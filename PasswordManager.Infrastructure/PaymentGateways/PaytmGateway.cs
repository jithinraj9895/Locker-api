using PasswordManager.Core.DTOs;

public class PaytmGateway : IPaymentGateway
{

    public string Name => "Paytm";

    public async Task<PaymentResult> CreatePaymentAsync(PaymentRequest paymentRequest)
    {
        //need to work on this in future
        return new PaymentResult
        {
            IsSuccess = true,
            Gateway = Name,
            PaymentId = Name + "id" + Guid.NewGuid()
        };
    }

    public async Task<PaymentResult> VerifyPaymentAsync(string paymentId)
    {
        //need to work on this in future
        return new PaymentResult
        {
            IsSuccess = true,
            Gateway = Name,
            PaymentId = Name + "id" + Guid.NewGuid()
        };
    }
}