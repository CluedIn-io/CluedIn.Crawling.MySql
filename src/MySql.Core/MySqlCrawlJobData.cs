using CluedIn.Core.Crawling;
using CluedIn.Crawling.MySql.Core.Models;
using System.Collections.Generic;

namespace CluedIn.Crawling.MySql.Core
{
    public class MySqlCrawlJobData : CrawlJobData
    {
        public string ConnectionString { get; set; }

        public List<TableMapping> TableMappings { get; set; }
    }
}
