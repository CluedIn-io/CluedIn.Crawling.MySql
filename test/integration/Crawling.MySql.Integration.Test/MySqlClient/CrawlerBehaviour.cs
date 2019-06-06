using Xunit;
using Xunit.Abstractions;

namespace Crawling.MySql.Integration.Test.MySqlClient
{
    public class CrawlerBehaviour : IClassFixture<MySqlTestFixture>
    {
        private readonly MySqlTestFixture _fixture;
        private readonly ITestOutputHelper _outputHelper;

        public CrawlerBehaviour(MySqlTestFixture fixture, ITestOutputHelper outputHelper)
        {
            _fixture = fixture;
            _outputHelper = outputHelper;
        }

        [Fact]
        public void CrawlerProducesClues()
        {
            Assert.True(_fixture.ClueStorage.HasClues());
        }
    }
}

