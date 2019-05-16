using CluedIn.Core.Crawling;
using CluedIn.Crawling;
using CluedIn.Crawling.MySql;
using CluedIn.Crawling.MySql.Infrastructure.Factories;
using Moq;
using Should;
using Xunit;

namespace Crawling.MySql.Test
{
    public class MySqlCrawlerBehaviour
    {
        private readonly ICrawlerDataGenerator _sut;

        private readonly Mock<IMySqlClientFactory> _nameClientFactory;

        public MySqlCrawlerBehaviour()
        {
            _nameClientFactory = new Mock<IMySqlClientFactory>();

            _sut = new MySqlCrawler(_nameClientFactory.Object);
        }

        [Fact]
        public void GetDataReturnsData()
        {
            var jobData = new CrawlJobData();

            _sut.GetData(jobData)
                .ShouldNotBeNull();
        }
    }
}
