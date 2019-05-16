using System.Collections.Generic;

using CluedIn.Core.Crawling;
using CluedIn.Crawling.MySql.Core;
using CluedIn.Crawling.MySql.Infrastructure.Factories;

namespace CluedIn.Crawling.MySql
{
    public class MySqlCrawler : ICrawlerDataGenerator
    {
        private readonly IMySqlClientFactory _clientFactory;
        public MySqlCrawler(IMySqlClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public IEnumerable<object> GetData(CrawlJobData jobData)
        {
            if (!(jobData is MySqlCrawlJobData mysqlcrawlJobData))
            {
                yield break;
            }

            var client = _clientFactory.CreateNew(mysqlcrawlJobData);

            //crawl data from provider and yield objects

            foreach( var folder in client.GetFolders())
            {
                yield return folder;
                foreach (var file in client.GetFilesForFolder(folder.Id))
                {
                    yield return file;
                }
            }
        }       
    }
}