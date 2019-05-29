using System;
using System.Configuration;
using AutoFixture.Xunit2;
using CluedIn.Core.Crawling;
using CluedIn.Crawling.MySql.Core;
using Provider.MySql.Unit.Test.MySqlProvider;
using Should;
using Xunit;

namespace Crawling.MySql.Integration.Test.MySqlProvider
{
    public class GetHelperConfigurationBehaviour : MySqlProviderTest
    {
        private readonly CrawlJobData _jobData;

        public GetHelperConfigurationBehaviour()
        {
            _jobData = new MySqlCrawlJobData
            {
                ConnectionString = ConfigurationManager.ConnectionStrings["SampleDatabase"].ConnectionString
            };
        }

        [Theory]
        [InlineAutoData]
        public void Returns_ValidDictionary_Instance(Guid organizationId, Guid userId, Guid providerDefinitionId)
        {
            Sut.GetHelperConfiguration(null, this._jobData, organizationId, userId, providerDefinitionId)
                .Result
                .ShouldNotBeNull();
        }
    }
}
