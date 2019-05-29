using System.Linq;
using Xunit;

namespace Crawling.MySql.Integration.Test.MySqlClient
{
    public class GetFolders : MySqlClientTestBase
    {
        [Fact(Skip = "MySql.Data.MySqlClient.MySqlException : Expression #4 of SELECT list is not in GROUP BY clause and contains nonaggregated column 'sakila.category.name' which is not functionally dependent on columns in GROUP BY clause; this is incompatible with sql_mode=only_full_group_by")]
        public void GetFoldersReturnsModelData()
        {
            var result = Sut.GetFolders().ToList();

            Assert.NotEmpty(result);
        }
    }
}
