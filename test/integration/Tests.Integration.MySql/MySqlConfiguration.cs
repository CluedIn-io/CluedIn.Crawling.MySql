using System.Collections.Generic;
using CluedIn.Crawling.MySql.Core;

namespace Tests.Integration.MySql
{
  public static class MySqlConfiguration
  {
    public static Dictionary<string, object> Create()
    {
      return new Dictionary<string, object>
            {
                { MySqlConstants.KeyName.ApiKey, "demo" }
            };
    }
  }
}
