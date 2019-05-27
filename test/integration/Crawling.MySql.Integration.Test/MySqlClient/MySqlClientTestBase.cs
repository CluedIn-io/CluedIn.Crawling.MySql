using CluedIn.Core.Logging;
using CluedIn.Crawling.MySql.Core;
using CluedIn.Crawling.MySql.Infrastructure;
using Moq;

namespace Crawling.MySql.Integration.Test
{
    public class MySqlClientTestBase
    {
        protected readonly MySqlClient Sut;

        private const string ConnectionString = "server=localhost;userid=root;password=sakila; database=sakila"; // TODO developer connection string

        public MySqlClientTestBase()
        {
            var logger = new Mock<ILogger>().Object;   // TODO Logger not used in MySqlClient ...

            Sut = new MySqlClient(logger, new MySqlCrawlJobData { ConnectionString = ConnectionString });
        }
    }
}
