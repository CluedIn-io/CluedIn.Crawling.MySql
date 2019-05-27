using Xunit;

namespace Crawling.MySql.Integration.Test.MySqlClient
{
    public class GetAccountInformation : MySqlClientTestBase
    {
        [Fact]
        public void GetAccountInformationReturnsData()
        {
            var result = Sut.GetAccountInformation();

            Assert.NotNull(result);
        }
    }
}
