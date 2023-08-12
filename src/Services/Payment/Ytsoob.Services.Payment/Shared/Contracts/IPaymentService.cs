using Ytsoob.Services.Payment.Shared.Models;
using Ytsoob.Services.Payment.Subscriptions.Models;
using Ytsoob.Services.Payment.Ytsoobers.Features.Models;

namespace Ytsoob.Services.Payment.Shared.Contracts;

public interface IPaymentService
{
    public Task<string> CreateStripeUser(Ytsoober ytsoober);
    public Task<CreateProductStripeResult> CreateSubProduct(Subscription subscription);
}
