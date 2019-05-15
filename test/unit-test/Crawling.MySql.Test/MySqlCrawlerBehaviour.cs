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

    public MySqlCrawlerBehaviour()
    {
        var nameClientFactory = new Mock<IMySqlClientFactory>();

        _sut = new MySqlCrawler(nameClientFactory.Object);
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
