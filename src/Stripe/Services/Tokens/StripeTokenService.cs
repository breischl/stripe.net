using Newtonsoft.Json;
using Stripe.Infrastructure;
using Stripe.Services;

namespace Stripe
{
	public class StripeTokenService : StripeServiceBase
	{
        public StripeTokenService() : base()
        {
        }

        public StripeTokenService(StripeConfiguration config)
            : base(config)
        {
        }

		public virtual StripeToken Create(StripeTokenCreateOptions createOptions)
		{
			var url = ParameterBuilder.ApplyAllParameters(createOptions, Urls.Tokens);

			var response = Requestor.PostString(url);

			return Mapper<StripeToken>.MapFromJson(response);
		}

		public virtual StripeToken Get(string tokenId)
		{
			var url = string.Format("{0}/{1}", Urls.Tokens, tokenId);

			var response = Requestor.GetString(url);

			return Mapper<StripeToken>.MapFromJson(response);
		}
	}
}