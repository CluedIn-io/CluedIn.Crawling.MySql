using System;
using System.Collections.Generic;
using CluedIn.Core.Net.Mail;
using CluedIn.Core.Providers;

namespace CluedIn.Crawling.MySql.Core
{
    public class MySqlConstants
    {
        public const string CodeOrigin = "MySql";
        public const string ProviderRootCodeValue = "MySql";
        public const string CrawlerName = "MySqlCrawler"; 
        public const string CrawlerComponentName = "MySqlCrawler";
        public const string CrawlerDescription = "MySql is a relational database"; // TODO
        public const string CrawlerDisplayName = "MySql";
        public const string Uri = "https://www.mysql.com/";


        public static readonly Guid ProviderId = Guid.Parse("620bbc66-0545-4490-a74d-bf2a2f4299be");   // TODO: Replace value
        public const string ProviderName = "MySql";         // TODO: Replace value
        public const bool SupportsConfiguration = true;             // TODO: Replace value
        public const bool SupportsWebHooks = true;             // TODO: Replace value
        public const bool SupportsAutomaticWebhookCreation = true;             // TODO: Replace value
        public const bool RequiresAppInstall = false;            // TODO: Replace value
        public const string AppInstallUrl = null;             // TODO: Replace value
        public const string ReAuthEndpoint = null;             // TODO: Replace value
        public const string IconUri = "https://s3-eu-west-1.amazonaws.com/staticcluedin/mysql.png"; // TODO: Replace value

        public static readonly ComponentEmailDetails ComponentEmailDetails = new ComponentEmailDetails
        {
            Features = new Dictionary<string, string>
            {
                                       { "Tracking",        "Expenses and Invoices against customers" },
                                       { "Intelligence",    "Aggregate types of invoices and expenses against customers and companies." }
                                   },
            Icon = new Uri(IconUri),
            ProviderName = ProviderName,
            ProviderId = ProviderId,
            Webhooks = SupportsWebHooks
        };

        public static IProviderMetadata CreateProviderMetadata()
        {
            return new ProviderMetadata
            {
                Id = ProviderId,
                ComponentName = CrawlerName,
                Name = ProviderName,
                Type = "Cloud",
                SupportsConfiguration = SupportsConfiguration,
                SupportsWebHooks = SupportsWebHooks,
                SupportsAutomaticWebhookCreation = SupportsAutomaticWebhookCreation,
                RequiresAppInstall = RequiresAppInstall,
                AppInstallUrl = AppInstallUrl,
                ReAuthEndpoint = ReAuthEndpoint,
                ComponentEmailDetails = ComponentEmailDetails
            };
        }    
    }
}
