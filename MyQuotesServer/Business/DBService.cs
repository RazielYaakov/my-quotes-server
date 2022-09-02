using MyQuotesServer.Configuration;
using MyQuotesServer.Models;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace MyQuotesServer.Business
{
    public class DBService
    {
        private readonly ILogger logger;
        private readonly IMongoDatabase database;

        public DBService(IOptions<MongoConfig> config, ILogger<DBService> logger)
        {
            this.logger = logger;

            var settings = MongoClientSettings.FromUrl(new MongoUrl(config.Value.ConnectionString));
            settings.SslSettings = new SslSettings { EnabledSslProtocols = SslProtocols.Tls12 };
            settings.RetryWrites = false;

            try
            {
                var client = new MongoClient(settings);
                database = client.GetDatabase(config.Value.Database);
                logger.LogInformation("Connected to database");
            }
            catch (Exception e)
            {
                logger.LogCritical(e, "Could not connect to database");
                throw e;
            }
        }

        public IMongoCollection<Quote> GetQuotesCollection(string collectionName)
        {
            return database.GetCollection<Quote>(collectionName);
        }
    }
}
