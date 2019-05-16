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

        private const string ConnectionString = "server=localhost;userid=jerrong;password=jerrong; database=sakila";

        public MySqlClient([NotNull] ILogger log, [NotNull] MySqlCrawlJobData mysqlCrawlJobData)
        {
            this.log = log ?? throw new ArgumentNullException(nameof(log));

            this.nameCrawlJobData = mysqlCrawlJobData ?? throw new ArgumentNullException(nameof(mysqlCrawlJobData));
        }

        public IEnumerable<Model> GetFolders()
        {
            var tables = new List<string>();

            using (var connection = new MySqlConnection(ConnectionString))
            {
                const string tableSql = "SELECT table_name FROM information_schema.tables where table_schema=\'sakila\';";

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
                using (var connection = new MySqlConnection(ConnectionString))
                {
                    connection.Open();

                    using (var cmd = new MySqlCommand())
                    {
                        cmd.Connection = connection;
                        cmd.CommandText = "SELECT * FROM @tableName";

                        var parameter = new MySqlParameter
                        {
                            ParameterName = "@tableName",
                            MySqlDbType = MySqlDbType.VarString,
                            Direction = ParameterDirection.Input,
                            Value = tableName
                        };

                        cmd.Parameters.Add(parameter);

                        using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            if (reader == null) continue;

                            while (reader.Read())
                            {
                                var columns = new List<object>();
                                var model = new Model();

                                for (var i = 0; i <= reader.FieldCount; i++)
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
            MySqlConnection connection = new MySqlConnection(nameCrawlJobData.ConnectionString);
            return new AccountInformation(connection.Database, this.nameCrawlJobData.ConnectionString);
        }
    }
}
