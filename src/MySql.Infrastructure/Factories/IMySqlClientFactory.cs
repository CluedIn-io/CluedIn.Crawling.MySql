using CluedIn.Crawling.MySql.Core;

namespace CluedIn.Crawling.MySql.Infrastructure.Factories
{
    public interface IMySqlClientFactory
    {
        MySqlClient CreateNew(MySqlCrawlJobData mysqlCrawlJobData);
    }
}
