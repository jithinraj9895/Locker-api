using PasswordManager.Core.DTOs;

public interface IPaymentGateway
{
    string Name { get; }
    Task<PaymentResult> CreatePaymentAsync(PaymentRequest paymentRequest);

    Task<PaymentResult> VerifyPaymentAsync(string paymentId);

}