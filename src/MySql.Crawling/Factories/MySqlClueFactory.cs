using System;
using CluedIn.Core;
using CluedIn.Core.Data;
using CluedIn.Crawling.Factories;
using CluedIn.Crawling.MySql.Core;
using RuleConstants = CluedIn.Core.Constants.Validation.Rules;

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

      var data = clue.Data.EntityData;
      data.Name = MySqlConstants.CrawlerName;
      data.Uri = new Uri(MySqlConstants.Uri);
      data.Description = MySqlConstants.CrawlerDescription;

      // TODO this should not be necessary
      clue.ValidationRuleSuppressions.AddRange(new[]
          {
            RuleConstants.METADATA_002_Uri_MustBeSet
          });

      return clue;
    }
  }
}
