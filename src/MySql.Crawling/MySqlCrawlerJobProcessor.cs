using System;
using CluedIn.Core.Agent.Jobs;
using CluedIn.Crawling.MySql.Core;

namespace CluedIn.Crawling.MySql
{
    public class MySqlCrawlerJobProcessor : GenericCrawlerTemplateJobProcessor<MySqlCrawlJobData>
    {
        public MySqlCrawlerJobProcessor(MySqlCrawlerComponent component) : base(component)
        {
        }
    }
}