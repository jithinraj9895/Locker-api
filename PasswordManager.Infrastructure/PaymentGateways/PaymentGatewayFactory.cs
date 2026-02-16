using PasswordManager.Core.Interfaces;

public class PaymentGatewayFactory : IPaymentGatewayFactory
{
    private readonly IEnumerable<IPaymentGateway> _paymentGateways;

    public PaymentGatewayFactory(IEnumerable<IPaymentGateway> paymentGateways)
    {
        _paymentGateways = paymentGateways;
    }
    public IPaymentGateway GetGateway(string gatewayName)
    {
        var gateway = _paymentGateways.FirstOrDefault(g => g.Name.Equals(gatewayName, StringComparison.OrdinalIgnoreCase));
        if (gateway == null)
        {
            throw new InvalidOperationException("no gateway found");
        }
        return gateway;
    }
}