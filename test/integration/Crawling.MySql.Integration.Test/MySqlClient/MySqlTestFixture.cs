using System.IO;
using System.Reflection;
using CluedIn.Crawling;
using CluedIn.Crawling.MySql.Core;

namespace Crawling.MySql.Integration.Test.MySqlClient
{
    public class MySqlTestFixture
    {
        public MySqlTestFixture()
        {
            var executingFolder = new FileInfo(Assembly.GetExecutingAssembly().CodeBase.Substring(8)).DirectoryName;
            var p = new DebugCrawlerHost<MySqlCrawlJobData>(executingFolder, MySqlConstants.ProviderName);
            ClueStorage = new ClueStorage();

            p.ProcessClue += ClueStorage.AddClue;

            p.Execute(MySqlConfiguration.Create(), MySqlConstants.ProviderId);
        }

        public ClueStorage ClueStorage { get; }
    }
}
