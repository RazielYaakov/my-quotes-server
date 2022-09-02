using MyQuotesServer.Configuration;
using MyQuotesServer.Models;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyQuotesServer.Business
{
    public class QuotesService
    {
        private readonly IMongoCollection<Quote> quotesCollection;
        private readonly ILogger logger;

        public QuotesService(IOptions<MongoConfig> config, ILogger<QuotesService> logger, DBService dBService)
        {
            this.logger = logger;

            try
            {
                quotesCollection = dBService.GetQuotesCollection(config.Value.QuotesCollection);
                logger.LogInformation($"Connected to Quote collection");
            }
            catch (Exception e)
            {
                logger.LogCritical(e, "Could not connect to database");
                throw e;
            }
        }

        public async Task<List<Quote>> GetAll()
        {
            try
            {
                var quotes = await quotesCollection.Find(Builders<Quote>.Filter.Empty).ToListAsync();
                logger.LogInformation("Got information of quotes from DB");

                return quotes;
            }
            catch (Exception e)
            {
                logger.LogError(e, $"Could not get all document");
                return new List<Quote>();
            }
        }

        public async Task<Quote> GetQuote(string quoteID)
        {
            try
            {
                logger.LogInformation($"Trying to get quote {quoteID} from DB");
                var quote = await quotesCollection.Find(quote => quote.Id == quoteID).ToListAsync();

                if(quote.FirstOrDefault().Id == null) {
                    logger.LogInformation($"Not found information about quote {quoteID}");
                    return null;
                }

                logger.LogInformation("Got information of quote from DB");
                
                return quote.FirstOrDefault();
            }
            catch (Exception e)
            {
                logger.LogError(e, $"Could not get quote");
                return null;
            }
        }

        public async Task Create(Quote quote)
        {
            try
            {
                await quotesCollection.InsertOneAsync(quote);
                logger.LogInformation("Added quote to DB");
            }
            catch (Exception e)
            {
                logger.LogError(e, $"Could not create document Id of {quote.Id}");
            }
        }

        public async Task Delete(string quoteID)
        {
            try
            {
                await quotesCollection.DeleteOneAsync(quote => quote.Id == quoteID);
                logger.LogInformation("Quote deleted from DB");
            }
            catch (Exception e)
            {
                logger.LogError(e, $"Could not create document Id of {quoteID}");
            }
        }
    }
}
