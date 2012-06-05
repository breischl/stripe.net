using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using Stripe.Infrastructure;

namespace Stripe
{
	internal class Requestor
	{
		private StripeConfiguration config;
		private string AuthorizationHeaderValue;

		internal Requestor(StripeConfiguration config)
		{
			this.config = config;
			this.AuthorizationHeaderValue = GetAuthorizationHeaderValue(config.ApiKey);
		}

		public string GetString(string url)
		{
			var wr = GetWebRequest(url, "GET");

			return ExecuteWebRequest(wr);
		}

		public string PostString(string url)
		{
			var wr = GetWebRequest(url, "POST");

			return ExecuteWebRequest(wr);
		}

		public string Delete(string url)
		{
			var wr = GetWebRequest(url, "DELETE");

			return ExecuteWebRequest(wr);
		}

		private WebRequest GetWebRequest(string url, string method)
		{
			var request = (HttpWebRequest) WebRequest.Create(url);
			request.Method = method;
			request.Headers.Add("Authorization", AuthorizationHeaderValue);
			request.ContentType = "application/x-www-form-urlencoded";
			request.UserAgent = "Stripe.net (https://github.com/jaymedavis/stripe.net)";

			return request;
		}

		private string GetAuthorizationHeaderValue(string apiKey)
		{
			var token = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:", apiKey)));
			return string.Format("Basic {0}", token);
		}

		private string ExecuteWebRequest(WebRequest webRequest)
		{
			try
			{
				using(var response = webRequest.GetResponse())
				{
					return ReadStream(response.GetResponseStream());
				}
			}
			catch(WebException webException)
			{
				if (webException.Response != null)
				{
					var statusCode = ((HttpWebResponse) webException.Response).StatusCode;
					var stripeError = Mapper<StripeError>.MapFromJson(ReadStream(webException.Response.GetResponseStream()), "error");

					throw new StripeException(statusCode, stripeError, stripeError.Message);
				}

				throw;
			}
		}

		private string ReadStream(Stream stream)
		{
			using (var reader = new StreamReader(stream, Encoding.UTF8))
			{
				return reader.ReadToEnd();
			}
		}
	}
}