namespace PasswordManager.Core.Interfaces;

public interface IPaymentGatewayFactory
{
    IPaymentGateway GetGateway(string gatewayName);
}
