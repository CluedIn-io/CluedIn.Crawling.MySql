using System.IO;
using System.Reflection;
using Castle.Windsor;
using CluedIn.Core.Data;
using CluedIn.Crawler.Console.Crawling;
using CluedIn.Crawling.MySql.Core;
using Moq;
using Serilog;
using Xunit;
using Xunit.Abstractions;

namespace Crawling.MySql.Integration.Test
{
    public class MySqlCrawlerBehaviour
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly CrawlerHost<MySqlCrawlJobData> _sut;

        public MySqlCrawlerBehaviour(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;

            var executingFolder = new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName;
            var windsorContainer = new WindsorContainer();

            var logger = new Mock<ILogger>();

            _sut = new CrawlerHost<MySqlCrawlJobData>(windsorContainer, logger.Object, executingFolder, $"*{MySqlConstants.ProviderName}*");

            _sut.ProcessClue += MethodDoingSomethingWithClue;
        }

        [Fact]
        public void CrawlerHostCanBeStarted()
        {
            Assert.NotNull(_sut);
        }

        [Fact]
        public void ExecuteCrawlerHostDoesNotThrow ()
        {
            _sut.Execute(MySqlConfiguration.Create(),MySqlConstants.ProviderId);
        }

        private void MethodDoingSomethingWithClue(Clue clue)
        {
            _testOutputHelper.WriteLine($"Clue ID: {clue.OriginEntityCode.Value} Object: {clue.Serialize()}");
        }
    }
}
