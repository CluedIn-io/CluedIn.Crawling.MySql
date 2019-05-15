using Xunit;
using System.Linq;
using CrawlerIntegrationTesting.Clues;
using Xunit.Abstractions;

namespace Tests.Integration.MySql
{
    public class DataIngestion : IClassFixture<MySqlTestFixture>
    {
        private const string AtLeastOnPropertyMustExist =
            "CluedIn.Core.Data.Validation.ClueValidationException Clue validation rule PROPERTIES_001_MustExist failed: At least one property must exist";

        private readonly MySqlTestFixture _fixture;
        private readonly ITestOutputHelper _output;

        public DataIngestion(MySqlTestFixture fixture, ITestOutputHelper output)
        {
            _fixture = fixture;
            _output = output;
            
        }

        [Theory(Skip = AtLeastOnPropertyMustExist)]
        [InlineData("/Provider/Root", 1)] 
        [InlineData("/Files/Directory", 1)]
        [InlineData("/Files/File", 2)]
        public void CorrectNumberOfEntityTypes(string entityType, int expectedCount)
        {
            var foundCount = _fixture.ClueStorage.CountOfType(entityType);
            Assert.Equal(expectedCount, foundCount);
        }

        [Fact(Skip = AtLeastOnPropertyMustExist)]
        public void EntityCodesAreUnique()
        {            
            var count = _fixture.ClueStorage.Clues.Count();
            var unique = _fixture.ClueStorage.Clues.Distinct(new ClueComparer()).Count();

            //You could use this method to output info of all clues
            PrintClues();

            Assert.Equal(unique, count);
        }

        private void PrintClues()
        {
            foreach(var clue in _fixture.ClueStorage.Clues)
            {
                _output.WriteLine(clue.OriginEntityCode.ToString());
            }
        }
    }
}
