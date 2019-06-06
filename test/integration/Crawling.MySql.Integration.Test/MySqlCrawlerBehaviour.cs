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
        private Mock<ILogger> _logger;

        public MySqlCrawlerBehaviour(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void ExecuteCrawlerHostDoesNotThrow ()
        {
            var executingFolder = new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName;
            var windsorContainer = new WindsorContainer();
            _logger = new Mock<ILogger>();
            var p = new CrawlerHost<MySqlCrawlJobData>(windsorContainer, _logger.Object, executingFolder, MySqlConstants.ProviderName);

            p.ProcessClue += MethodDoingSomethingWithClue;

            p.Execute(MySqlConfiguration.Create(),MySqlConstants.ProviderId);
        }

        private void MethodDoingSomethingWithClue(Clue clue)
        {
            _testOutputHelper.WriteLine($"Clue ID: {clue.OriginEntityCode.Value} Object: {clue.Serialize()}");
        }
    }
}
