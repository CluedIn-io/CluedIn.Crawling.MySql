using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using CluedIn.Core;
using CluedIn.Core.Crawling;
using CluedIn.Core.Data.Relational;
using CluedIn.Core.Providers;
using CluedIn.Core.Webhooks;
using System.Configuration;
using CluedIn.Core.Configuration;
using CluedIn.Crawling.MySql.Core;
using CluedIn.Crawling.MySql.Infrastructure.Factories;
using CluedIn.Providers.Models;

namespace CluedIn.Provider.MySql
{
  public class MySqlProvider : ProviderBase
  {
    private readonly IMySqlClientFactory _mysqlClientFactory;

    /**********************************************************************************************************
     * CONSTRUCTORS
     **********************************************************************************************************/

    public MySqlProvider([NotNull] ApplicationContext appContext, IMySqlClientFactory mysqlClientFactory)
        : base(appContext, MySqlConstants.CreateProviderMetadata())
    {
      _mysqlClientFactory = mysqlClientFactory;
    }

    /**********************************************************************************************************
     * METHODS
     **********************************************************************************************************/

    public override async Task<CrawlJobData> GetCrawlJobData(
        ProviderUpdateContext context,
        IDictionary<string, object> configuration,
        Guid organizationId,
        Guid userId,
        Guid providerDefinitionId)
    {
      if (configuration == null) throw new ArgumentNullException(nameof(configuration));

      var mysqlCrawlJobData = new MySqlCrawlJobData();
      if (configuration.ContainsKey(MySqlConstants.KeyName.ApiKey))
      { mysqlCrawlJobData.ApiKey = configuration[MySqlConstants.KeyName.ApiKey].ToString(); }

      return await Task.FromResult(mysqlCrawlJobData);
    }

    public override Task<bool> TestAuthentication(
        ProviderUpdateContext context,
        IDictionary<string, object> configuration,
        Guid organizationId,
        Guid userId,
        Guid providerDefinitionId)
    {
      throw new NotImplementedException();
    }

    public override Task<ExpectedStatistics> FetchUnSyncedEntityStatistics(ExecutionContext context, IDictionary<string, object> configuration, Guid organizationId, Guid userId, Guid providerDefinitionId)
    {
      throw new NotImplementedException();
    }

    public override async Task<IDictionary<string, object>> GetHelperConfiguration(
        ProviderUpdateContext context,
        [NotNull] CrawlJobData jobData,
        Guid organizationId,
        Guid userId,
        Guid providerDefinitionId)
    {
      if (jobData == null) throw new ArgumentNullException(nameof(jobData));

      var dictionary = new Dictionary<string, object>();

      if (jobData is MySqlCrawlJobData mysqlCrawlJobData)
      {
        //TODO add the transformations from specific CrawlJobData object to dictionary
        // add tests to GetHelperConfigurationBehaviour.cs
        dictionary.Add(MySqlConstants.KeyName.ApiKey, mysqlCrawlJobData.ApiKey);
      }

      return await Task.FromResult(dictionary);
    }

    public override Task<IDictionary<string, object>> GetHelperConfiguration(
        ProviderUpdateContext context,
        CrawlJobData jobData,
        Guid organizationId,
        Guid userId,
        Guid providerDefinitionId,
        string folderId)
    {
      throw new NotImplementedException();  // TODO should this method be async ?
    }

    public override async Task<AccountInformation> GetAccountInformation(ExecutionContext context, [NotNull] CrawlJobData jobData, Guid organizationId, Guid userId, Guid providerDefinitionId)
    {
      if (jobData == null) throw new ArgumentNullException(nameof(jobData));

      if (!(jobData is MySqlCrawlJobData mysqlCrawlJobData))
      {
        throw new Exception("Wrong CrawlJobData type");
      }

      var client = _mysqlClientFactory.CreateNew(mysqlCrawlJobData);
      return await Task.FromResult(client.GetAccountInformation());
    }

    public override string Schedule(DateTimeOffset relativeDateTime, bool webHooksEnabled)
    {
      //TODO is this common for all providers?
      return webHooksEnabled && ConfigurationManager.AppSettings.GetFlag("Feature.Webhooks.Enabled", false) ? $"{relativeDateTime.Minute} 0/23 * * *"
          : $"{relativeDateTime.Minute} 0/4 * * *";
    }

    public override Task<IEnumerable<WebHookSignature>> CreateWebHook(ExecutionContext context, [NotNull] CrawlJobData jobData, [NotNull] IWebhookDefinition webhookDefinition, [NotNull] IDictionary<string, object> config)
    {
      if (jobData == null) throw new ArgumentNullException(nameof(jobData));
      if (webhookDefinition == null) throw new ArgumentNullException(nameof(webhookDefinition));
      if (config == null) throw new ArgumentNullException(nameof(config));

      throw new NotImplementedException();
    }

    public override Task<IEnumerable<WebhookDefinition>> GetWebHooks(ExecutionContext context)
    {
      throw new NotImplementedException();
    }

    public override Task DeleteWebHook(ExecutionContext context, [NotNull] CrawlJobData jobData, [NotNull] IWebhookDefinition webhookDefinition)
    {
      if (jobData == null) throw new ArgumentNullException(nameof(jobData));
      if (webhookDefinition == null) throw new ArgumentNullException(nameof(webhookDefinition));

      throw new NotImplementedException();
    }

    public override IEnumerable<string> WebhookManagementEndpoints([NotNull] IEnumerable<string> ids)
    {
      if (ids == null) throw new ArgumentNullException(nameof(ids));

      // TODO should ids also be checked for being empty ?

      throw new NotImplementedException();
    }

    public override async Task<CrawlLimit> GetRemainingApiAllowance(ExecutionContext context, [NotNull] CrawlJobData jobData, Guid organizationId, Guid userId, Guid providerDefinitionId)
    {
      if (jobData == null) throw new ArgumentNullException(nameof(jobData));

      //TODO what the hell is this?
      //There is no limit set, so you can pull as often and as much as you want.
      return await Task.FromResult(new CrawlLimit(-1, TimeSpan.Zero));
    }
  }
}