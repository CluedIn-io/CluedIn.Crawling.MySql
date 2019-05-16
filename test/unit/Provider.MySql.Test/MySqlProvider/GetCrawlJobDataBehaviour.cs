using AutoFixture.Xunit2;
using System;
using System.Collections.Generic;
using CluedIn.Core;
using Xunit;

namespace Provider.MySql.Test.MySqlProvider
{
    public class GetCrawlJobDataBehaviour : MySqlProviderTest
    {
        [Theory(Skip = "ExecutionContext not assignable to ProviderUpdateContext")]
        [InlineAutoData]
        public void GetCrawlJobDataTests(ExecutionContext context, Dictionary<string, object> dictionary, Guid organizationId, Guid userId, Guid providerDefinitionId)
        {
            // TODO restore test .... Sut.GetCrawlJobData(context, dictionary, organizationId, userId, providerDefinitionId).ShouldNotBeNull();
        }
    }
}
