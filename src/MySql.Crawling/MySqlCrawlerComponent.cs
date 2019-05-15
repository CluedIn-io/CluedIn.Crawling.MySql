using CluedIn.Core;
using CluedIn.Crawling.MySql.Core;

using ComponentHost;

namespace CluedIn.Crawling.MySql
{
    [Component(MySqlConstants.CrawlerComponentName, "Crawlers", ComponentType.Service, Components.Server, Components.ContentExtractors, Isolation = ComponentIsolation.NotIsolated)]
    public class MySqlCrawlerComponent : CrawlerComponentBase
    {
        public MySqlCrawlerComponent([NotNull] ComponentInfo componentInfo)
            : base(componentInfo)
        {
        }
    }
}

