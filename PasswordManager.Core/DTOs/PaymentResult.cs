namespace PasswordManager.Core.DTOs;

public class PaymentResult
{
    public bool IsSuccess { get; set; }
    public string Gateway { get; set; } = string.Empty;
    public string PaymentId { get; set; } = string.Empty;
    public string? ErrorMessage { get; set; }
}
