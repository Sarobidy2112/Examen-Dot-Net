using Stripe;
using Stripe.Checkout;

public class StripeService
{
    private readonly IConfiguration _config;

    public StripeService(IConfiguration config)
    {
        _config = config;
        StripeConfiguration.ApiKey = _config["Stripe:SecretKey"];
    }

    public async Task<Session> CreateCheckoutSession(List<SessionLineItemOptions> lineItems, string successUrl, string cancelUrl, string? customerEmail = null)
    {
        var options = new SessionCreateOptions
        {
            PaymentMethodTypes = new List<string> { "card" },
            LineItems = lineItems,
            Mode = "payment",
            SuccessUrl = successUrl,
            CancelUrl = cancelUrl,
            CustomerEmail = customerEmail
        };

        var service = new SessionService();
        return await service.CreateAsync(options);
    }
}