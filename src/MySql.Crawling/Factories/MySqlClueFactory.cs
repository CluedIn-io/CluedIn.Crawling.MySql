using System;
using CluedIn.Core;
using CluedIn.Core.Data;
using CluedIn.Crawling.Factories;
using CluedIn.Crawling.MySql.Core;

namespace CluedIn.Crawling.MySql.Factories
{
    public class MySqlClueFactory : ClueFactory
    {
        public MySqlClueFactory()
            : base(MySqlConstants.CodeOrigin, MySqlConstants.ProviderRootCodeValue)
        {
        }

        protected override Clue ConfigureProviderRoot([NotNull] Clue clue)
        {
            if (clue == null) throw new ArgumentNullException(nameof(clue));

            var data = clue.Data;
            data.EntityData.Name = MySqlConstants.CrawlerName;
            data.EntityData.Uri = new Uri(MySqlConstants.Uri);
            data.EntityData.Description = MySqlConstants.CrawlerDescription;
            
            return clue;
        }
    }
}
