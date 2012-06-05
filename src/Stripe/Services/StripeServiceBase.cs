using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stripe.Infrastructure;
using System.Configuration;

namespace Stripe.Services
{
    public abstract class StripeServiceBase
    {
        internal StripeServiceBase(StripeConfiguration config)
        {
            this.Config = config;
            this.Requestor = new Requestor(config);
        }

        internal StripeServiceBase() : this(StripeConfiguration.FromAppSettings())
        {
        }
        
        internal StripeConfiguration Config { get; private set; }

        internal Requestor Requestor
        {
            get;
            private set;
        }
    }
}
