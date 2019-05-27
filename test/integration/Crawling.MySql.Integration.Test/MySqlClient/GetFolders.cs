using Xunit;

namespace Crawling.MySql.Integration.Test.MySqlClient
{
    public class GetFolders : MySqlClientTestBase
    {
        [Fact]
        public void GetFoldersReturnsModelData()
        {
            var result = Sut.GetFolders();

            Assert.NotEmpty(result);
        }
    }
}
