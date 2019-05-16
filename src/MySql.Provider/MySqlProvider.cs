using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using CluedIn.Core;
using CluedIn.Core.Configuration;
using CluedIn.Core.Crawling;
using CluedIn.Core.Data;
using CluedIn.Core.Data.Relational;
using CluedIn.Core.Providers;
using CluedIn.Core.Webhooks;
using CluedIn.Crawling.MySql.Core;
using CluedIn.Crawling.MySql.Core.Models;
using CluedIn.Crawling.MySql.Infrastructure.Factories;
using CluedIn.Providers.Models;
using MySql.Data.MySqlClient;

namespace CluedIn.Provider.MySql
{
    public class MySqlProvider : ProviderBase
    {
        private readonly IMySqlClientFactory clientFactory;

        public MySqlProvider([NotNull] ApplicationContext appContext, IMySqlClientFactory clientFactory)
            : base(appContext, MySqlConstants.CreateProviderMetadata())
        {
            this.clientFactory = clientFactory;
        }

        public override async Task<CrawlJobData> GetCrawlJobData(ProviderUpdateContext context, [NotNull] IDictionary<string, object> configuration, Guid organizationId, Guid userId, Guid providerDefinitionId)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            var crawlJobData = new MySqlCrawlJobData();

            if (configuration.ContainsKey("connectionString"))
            {
                crawlJobData.ConnectionString = configuration["connectionString"].ToString();
            }

            if (configuration.ContainsKey("tableMappings"))
            {
                crawlJobData.TableMappings = JsonUtility.Deserialize<List<TableMapping>>(configuration["tableMappings"].ToString());
            }

            return await Task.FromResult(crawlJobData);
        }

        public override async Task<IDictionary<string, object>> GetHelperConfiguration(ProviderUpdateContext context, [NotNull] CrawlJobData jobData, Guid organizationId, Guid userId, Guid providerDefinitionId)
        {
            if (jobData == null) throw new ArgumentNullException(nameof(jobData));

            var crawlJobData = jobData as MySqlCrawlJobData;

            var dictionary = new Dictionary<string, object>();

            if (crawlJobData == null)
            {
                return await Task.FromResult(dictionary);
            }

            var tables = new List<string>();

            using (var connection = new MySqlConnection(crawlJobData.ConnectionString))
            {
                using (var command = new MySqlCommand(
                    "SELECT table_name FROM information_schema.tables where table_schema='@database'",
                    connection))
                {
                    command.Parameters.AddWithValue("database", connection.Database);

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tables.Add(reader["SchemaTable"].ToString());
                        }
                    }
                }
            }

            foreach (var table in tables)
            {
                var columns = new List<Column>();
                var keys = new List<Key>();

                var tableMapping = new TableMapping {
                    Table = table
                };

                using (var connection = new MySqlConnection(crawlJobData.ConnectionString))
                {
                    try
                    {
                        using (var command = new MySqlCommand(
                            "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = '@table' AND TABLE_NAME = '@database';",
                            connection))
                        {
                            command.Parameters.AddWithValue("table", table);
                            command.Parameters.AddWithValue("database", connection.Database);

                            using (var reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    var name = reader["Field"].ToString();
                                    var dataType = reader["Type"].ToString();
                                    var @default = reader["Default"].ToString();
                                    var isNullable = reader.GetBoolean(reader.GetOrdinal("Null"));

                                    var column = new Column {
                                        Name = name,
                                        DataType = dataType,
                                        Default = @default,
                                        IsNullable = isNullable
                                    };

                                    columns.Add(column);
                                }
                            }
                        }
                    }
                    catch (Exception exc)
                    {
                        Console.WriteLine(exc);
                    }

                    tableMapping.Columns = columns;
                }

                using (var connection = new MySqlConnection(crawlJobData.ConnectionString))
                {
                    try
                    {
                        const string query = "DESCRIBE products";

                        using (var cmd = new MySqlCommand(query, connection))
                        {
                            using (var reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    var name = reader["Field"].ToString();

                                    var key = new Key {
                                        Name = name,
                                        IsForeign = reader["Key"].ToString() == "UNI",
                                        IsPrimary = reader["Key"].ToString() == "PRI",
                                        FieldSource = reader["Field"].ToString()
                                    };

                                    keys.Add(key);
                                }
                            }
                        }
                    }
                    catch (Exception exc)
                    {
                        Console.WriteLine(exc);
                    }

                    tableMapping.Keys = keys;
                }
            }

            var fields = new List<string> {
                "Name",
                "DisplayName",
                "CreatedDate",
                "ModifiedDate",
                "Description",
                "Url",
                "Version",
                "PreviewImage",
                "Revision"
            };

            //Map to Entity e.g. What is name / vocabs / Created / Edge.
            dictionary.Add("clueFields", fields);

            dictionary.Add("edgeTypes", EntityEdgeType.AllCodes);

            dictionary.Add("entityTypes", EntityType.AllCodes);

            dictionary.Add("connectionString", crawlJobData.ConnectionString);

            return dictionary;
        }

        public override Task<IDictionary<string, object>> GetHelperConfiguration(
            ProviderUpdateContext context,
            CrawlJobData jobData,
            Guid organizationId,
            Guid userId,
            Guid providerDefinitionId,
            string folderId)
        {
            return Task.FromResult(default(IDictionary<string, object>));  // TODO implement .. MySqlProvider.GetHelperConfiguration(...)
        }

        public override async Task<AccountInformation> GetAccountInformation(ExecutionContext context, [NotNull] CrawlJobData jobData, Guid organizationId, Guid userId, Guid providerDefinitionId)
        {
            if (jobData == null) throw new ArgumentNullException(nameof(jobData));

            if (!(jobData is MySqlCrawlJobData crawlJobData))
            {
                throw new Exception("Wrong CrawlJobData type");
            }

            var client = this.clientFactory.CreateNew(crawlJobData);

            return await Task.FromResult(client.GetAccountInformation());
        }

        public override string Schedule(DateTimeOffset relativeDateTime, bool webHooksEnabled)
        {
            // TODO is this common for all providers?
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
            return Task.FromResult(Enumerable.Empty<WebhookDefinition>()); // TODO implement..  MySqlProvider.GetWebHooks(ExecutionContext context)
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

            return Enumerable.Empty<string>();  // TODO implement .. MySqlProvider.WebhookManagementEndpoints(IEnumerable<string> ids)
        }

        public override async Task<CrawlLimit> GetRemainingApiAllowance(ExecutionContext context, [NotNull] CrawlJobData jobData, Guid organizationId, Guid userId, Guid providerDefinitionId)
        {
            if (jobData == null) throw new ArgumentNullException(nameof(jobData));

            // TODO what the hell is this?
            //There is no limit set, so you can pull as often and as much as you want.
            return await Task.FromResult(new CrawlLimit(-1, TimeSpan.Zero));
        }

        public override async Task<bool> TestAuthentication(ProviderUpdateContext context, IDictionary<string, object> configuration, Guid organizationId, Guid userId, Guid providerDefinitionId)
        {
            var crawlJobData = new MySqlCrawlJobData();

            if (configuration.ContainsKey("connectionString")) { crawlJobData.ConnectionString = configuration["connectionString"].ToString(); }
            if (configuration.ContainsKey("tableMappings")) { crawlJobData.TableMappings = JsonUtility.Deserialize<List<TableMapping>>(configuration["tableMappings"].ToString()); }

            using (var connection = new MySqlConnection(crawlJobData.ConnectionString))
            {
                try
                {
                    var tableSql =
                        $"SELECT table_name FROM information_schema.tables where table_schema='{connection.Database}';";

                    using (var command = new MySqlCommand(tableSql, connection))
                    {
                        connection.Open();

                        await command.ExecuteNonQueryAsync();
                    }
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return true;
        }

        public override Task<ExpectedStatistics> FetchUnSyncedEntityStatistics(ExecutionContext context, IDictionary<string, object> configuration, Guid organizationId, Guid userId, Guid providerDefinitionId)
        {
            return Task.FromResult(default(ExpectedStatistics));  // TODO implement .. MySqlProvider.FetchUnSyncedEntityStatistics(...)
        }
    }
}
