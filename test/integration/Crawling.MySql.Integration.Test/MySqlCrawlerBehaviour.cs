using System.IO;
using System.Reflection;
using CluedIn.Core.Data;
using CluedIn.CrawlerMySql.Console;
using CluedIn.Crawling;
using CluedIn.Crawling.MySql.Core;
using Xunit;

namespace Crawling.MySql.Integration.Test
{
    public class MySqlCrawlerBehaviour
    {
        [Fact(Skip = "System.NullReferenceException. Object reference not set to an instance of an object.")]
        public void RunAllTests ()
        {
            var executingFolder = new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName;
            var p = new DebugCrawlerHost<MySqlCrawlJobData>(executingFolder, MySqlConstants.ProviderName);

            p.ProcessClue += MethodDoingSomethingWithClue;

            p.Execute(MySqlConfiguration.Create(),MySqlConstants.ProviderId);
        }

        private static void MethodDoingSomethingWithClue(Clue clue)
        {
            // This is your opportunity to add custom actions for clue processing testing
            var info = clue.Serialize(); // this outputs the full data of the clue. Useful for debugging.
           // var info = clue.OriginEntityCode.Value; //Just outputs the ID of the clue
            System.Console.WriteLine(info);
        }
    }
}
