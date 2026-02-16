namespace PasswordManager.Core.DTOs;

public class PaymentRequest
{
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "INR";
    public string OrderId { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
