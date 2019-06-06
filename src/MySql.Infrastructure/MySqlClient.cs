// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MySqlClient.cs" company="Clued In">
//   Copyright (c) 2018 Clued In. All rights reserved.
// </copyright>
// <summary>
//   Implements my SQL client class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;

using CluedIn.Core;
using CluedIn.Core.Logging;
using CluedIn.Core.Providers;
using CluedIn.Crawling.MySql.Core;
using CluedIn.Crawling.MySql.Core.Models;

using MySql.Data.MySqlClient;

namespace CluedIn.Crawling.MySql.Infrastructure
{
    public class MySqlClient
    {
        private readonly ILogger log;

        private readonly MySqlCrawlJobData nameCrawlJobData;

        public MySqlClient([NotNull] ILogger log, [NotNull] MySqlCrawlJobData mysqlCrawlJobData)
        {
            this.log = log ?? throw new ArgumentNullException(nameof(log));

            nameCrawlJobData = mysqlCrawlJobData ?? throw new ArgumentNullException(nameof(mysqlCrawlJobData));
        }

        public IEnumerable<Model> GetFolders()
        {
            var tables = new List<string>();

            using (var connection = new MySqlConnection(nameCrawlJobData.ConnectionString))
            {
                const string tableSql = "SELECT table_name FROM information_schema.tables where table_schema=\'sakila\';";  // TODO reference to MySQL sample database, ref: https://dev.mysql.com/doc/sakila/en/

                connection.Open();

                using (var command = new MySqlCommand(tableSql, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tables.Add(reader["table_name"].ToString());
                        }
                    }
                }
            }

            foreach (var tableName in tables)
            {
                var cmdBuilder = new MySqlCommandBuilder();
                string tbName = cmdBuilder.QuoteIdentifier(tableName);

                using (var connection = new MySqlConnection(nameCrawlJobData.ConnectionString))
                {
                    connection.Open();

                    using (var cmd = new MySqlCommand())
                    {
                        cmd.Connection = connection;
                        cmd.CommandText = $"SELECT * FROM {tbName}";

                        using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            if (reader == null) continue;

                            while (reader.Read())
                            {
                                var columns = new List<object>();
                                var model = new Model();

                                for (var i = 0; i < reader.FieldCount; i++)
                                {
                                    columns.Add(reader[i].ToString());
                                }

                                model.Columns = columns;

                                yield return model;
                            }
                        }
                    }
                }
            }
        }

        public AccountInformation GetAccountInformation()
        {
            var connection = new MySqlConnection(nameCrawlJobData.ConnectionString);
            return new AccountInformation(connection.Database, nameCrawlJobData.ConnectionString);
        }
    }
}
