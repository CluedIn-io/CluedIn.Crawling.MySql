using Xunit;

namespace Crawling.MySql.Integration.Test
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
