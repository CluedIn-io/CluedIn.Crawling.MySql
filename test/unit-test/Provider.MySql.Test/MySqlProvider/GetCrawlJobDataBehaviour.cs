using AutoFixture.Xunit2;
using Should;
using System;
using System.Collections.Generic;

using Xunit;

namespace Provider.MySql.Test.MySqlProvider
{
  public class GetCrawlJobDataBehaviour : MySqlProviderTest
  {
    [Theory]
    [InlineAutoData]
    public void GetCrawlJobDataTests(Dictionary<string, object> dictionary, Guid organizationId, Guid userId, Guid providerDefinitionId)
    {
      //TODO: passing null here does not look good
      Sut.GetCrawlJobData(null, dictionary, organizationId, userId, providerDefinitionId)
          .ShouldNotBeNull();
    }
  }
}
