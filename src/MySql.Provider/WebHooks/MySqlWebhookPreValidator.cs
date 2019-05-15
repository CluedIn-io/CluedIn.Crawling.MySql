using CluedIn.Core.Webhooks;
using CluedIn.Crawling.MySql.Core;

namespace CluedIn.Provider.MySql.WebHooks
{
    public class Name_WebhookPreValidator : BaseWebhookPrevalidator
    {
        public Name_WebhookPreValidator()
            : base(MySqlConstants.ProviderId, MySqlConstants.ProviderName)
        {
        }
    }
}
