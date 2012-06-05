using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Stripe.Infrastructure
{
    public class StripeConfiguration
    {
        public StripeConfiguration(string apiKey)
        {
            this.ApiKey = apiKey;
        }

        public static StripeConfiguration FromAppSettings()
        {
            return new StripeConfiguration(ConfigurationManager.AppSettings["StripeApiKey"]);
        }

        public string ApiKey { get; private set; }
    }
}
