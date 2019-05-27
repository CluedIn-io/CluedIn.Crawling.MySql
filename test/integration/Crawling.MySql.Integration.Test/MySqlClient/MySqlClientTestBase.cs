using System.Configuration;
using CluedIn.Core.Logging;
using CluedIn.Crawling.MySql.Core;
using Moq;

namespace Crawling.MySql.Integration.Test.MySqlClient
{
    public class MySqlClientTestBase
    {
        protected readonly CluedIn.Crawling.MySql.Infrastructure.MySqlClient Sut;

        protected readonly Mock<ILogger> Logger;

        public MySqlClientTestBase()
        {
            Logger = new Mock<ILogger>();

            var crawlJobData = new MySqlCrawlJobData
            {
                ConnectionString = ConfigurationManager.ConnectionStrings["SampleDatabase"].ConnectionString
            };

            Sut = new CluedIn.Crawling.MySql.Infrastructure.MySqlClient(Logger.Object, crawlJobData);
        }
    }
}
