using System;
using System.Linq;
using CluedIn.Core.Crawling;
using AutoFixture.Xunit2;
using Should;
using Xunit;
using CluedIn.Crawling.MySql.Core;

namespace Provider.MySql.Test.MySqlProvider
{
    public class GetHelperConfigurationBehaviour : MySqlProviderTest
    {
        private readonly CrawlJobData _jobData;

        public GetHelperConfigurationBehaviour()
        {
            _jobData = new MySqlCrawlJobData();
        }

        [Fact]
        public void Throws_ArgumentNullException_With_Null_CrawlJobData_Parameter()
        {
            var ex = Assert.Throws<AggregateException>(
                () => Sut.GetHelperConfiguration(null, null, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid())
                    .Wait());

            ((ArgumentNullException)ex.InnerExceptions.Single())
                .ParamName
                .ShouldEqual("jobData");
        }

        // TODO Add test for throws arg exception for incorrect data param

        [Theory(Skip = "System.NullReferenceException : Object reference not set to an instance of an object")]
        [InlineAutoData("Sample-Property","SampleProperty","some-value")]
        // TODO add data for other properties that need populating
        //[InlineAutoData("Sample-Property2", "SampleProperty2", "some-value")]
        // Fill in the values for expected results ....
        public void Returns_Expected_Data(string key, string propertyName, object expectedValue, Guid organizationId, Guid userId, Guid providerDefinitionId) // TODO add additional parameters to populate CrawlJobData instance
        {
            // TODO populate CrawlJobData instance with additional parameters ...

            var property = _jobData.GetType().GetProperty(propertyName);
            property.SetValue(_jobData, expectedValue);
            
            var result = Sut.GetHelperConfiguration(null, this._jobData, organizationId, userId, providerDefinitionId)
                            .Result;

            result
                .ContainsKey(key)
                .ShouldBeTrue(
                    $"{key} not found in results");
            
            result[key]
                .ShouldEqual(expectedValue);
        }
    }
}
