namespace PasswordManager.Core.Interfaces;

using PasswordManager.Core.DTOs;

public interface IPaymentService
{
    Task<PaymentResult> CreatePaymentAsync(string gateway, PaymentRequest request);
}
