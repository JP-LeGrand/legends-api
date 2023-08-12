using System.Text;
using legend.Extensions;

namespace legend.PayFast
{
    public class PayFastApiBase
    {
        #region Fields

        private readonly PayFastSettings payfastSettings;

        #endregion Fields

        #region Constructor

        public PayFastApiBase(PayFastSettings payfastSettings)
        {
            this.payfastSettings = payfastSettings ?? throw new ArgumentNullException(nameof(payfastSettings));
        }

        #endregion Constructor

        #region Properties

        protected const string BaseUrl = "https://api.payfast.co.za/subscriptions/";
        protected const string ApiVersion = "v1";
        protected const string TestMode = "?testing=true";
        protected const string PingResourceUrl = "ping";
        private const string CancelResourceUrl = "cancel";

        #endregion Properties

        #region Methods

        protected HttpClient GetClient()
        {
            var httpClient = new HttpClient { BaseAddress = new Uri(BaseUrl) };

            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Add("merchant-id", this.payfastSettings.MerchantId);
            httpClient.DefaultRequestHeaders.Add("version", ApiVersion);
            httpClient.DefaultRequestHeaders.Add("timestamp", DateTime.UtcNow.ToString("s"));

            return httpClient;
        }

        /// <summary>
        /// This method will generate the signature for the request
        /// See <a href="https://www.payfast.co.za/documentation/api/#Merchant_Signature_Generation">PayFast API Signature Generation Documentation</a> for more information.
        /// </summary>
        protected string GenerateSignature(HttpClient httpClient, params KeyValuePair<string, string>[] parameters)
        {
            var dictionary = new SortedDictionary<string, string>();

            foreach (var header in httpClient.DefaultRequestHeaders)
            {
                dictionary.Add(key: header.Key, value: header.Value.First());
            }

            foreach (var keyValuePair in parameters)
            {
                dictionary.Add(key: keyValuePair.Key, value: keyValuePair.Value);
            }

            if (!string.IsNullOrWhiteSpace(this.payfastSettings.PassPhrase))
            {
                dictionary.Add(key: "passphrase", value: this.payfastSettings.PassPhrase);
            }

            var stringBuilder = new StringBuilder();
            var last = dictionary.Last();

            foreach (var keyValuePair in dictionary)
            {
                stringBuilder.Append($"{keyValuePair.Key.UrlEncode()}={keyValuePair.Value.UrlEncode()}");

                if (keyValuePair.Key != last.Key)
                {
                    stringBuilder.Append("&");
                }
            }

            httpClient.DefaultRequestHeaders.Add(name: "signature", value: stringBuilder.CreateHash());

            if (parameters.Length > 0)
            {
                var jsonStringBuilder = new StringBuilder();
                jsonStringBuilder.Append("{");

                var lastParameter = parameters.Last();

                foreach (var keyValuePair in parameters)
                {
                    jsonStringBuilder.Append($"\"{keyValuePair.Key}\" : \"{keyValuePair.Value}\"");

                    if (lastParameter.Key != keyValuePair.Key)
                    {
                        jsonStringBuilder.Append(",");
                    }
                }

                jsonStringBuilder.Append("}");

                return jsonStringBuilder.ToString();
            }
            else
            {
                return null;
            }
        }

        #endregion Methods
    }
}

