using System.Collections.Generic;

namespace CluedIn.CrawlerMySql.Console
{
    public static class MySqlConfiguration
    {
        public struct KeyName
        {
            public static readonly string ConsumerKey = "ConsumerKey";
            public static readonly string ConsumerSecret = "ConsumerSecret";
            public static readonly string OAuthToken = "OAuthToken";
            public static readonly string OAuthTokenSecret = "OAuthTokenSecret";
        }
        
        public static Dictionary<string,object> Create()
        {
            var credentials = new Dictionary<string,object>
            {
                { KeyName.ConsumerKey, "TODO" },
                { KeyName.ConsumerSecret, "TODO" },
                { KeyName.OAuthToken, "TODO" },
                { KeyName.OAuthTokenSecret, "TODO" }
            };

            return credentials;
        }
    }
}
