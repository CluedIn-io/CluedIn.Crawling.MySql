using System.Collections.Generic;

namespace Crawling.MySql.Integration.Test
{
    public static class MySqlConfiguration
    {
        public static Dictionary<string,object> Create()
        {
            var credentials = new Dictionary<string,object>
            {
                { "connectionString" , "server=localhost;userid=root;password=sakila; database=sakila" }
            };

            return credentials;
        }
    }
}
