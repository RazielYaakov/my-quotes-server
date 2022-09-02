using System.Collections.Generic;

namespace MyQuotesServer.Configuration
{
    public class MongoConfig
    {
        public string ConnectionString { get; set; }
        public string Database { get; set; }
        public string QuotesCollection { get; set; }
    }
}
