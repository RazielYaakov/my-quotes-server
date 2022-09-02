using MyQuotesServer.Business;
using MyQuotesServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyQuotesServer.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class QuotesController : ControllerBase
    {
        private readonly QuotesService quotesService;
        private readonly ILogger logger;

        public QuotesController(QuotesService quotesService, ILogger<QuotesController> logger)
        {
            this.quotesService = quotesService;
            this.logger = logger;
        }

        [HttpPost()]
        [ProducesResponseType(typeof(string), 200)]
        public async Task<IActionResult> AddQuote([FromBody] Quote quoteData)
        {
            logger.LogInformation($"Adding quote {quoteData.Id}");

            await quotesService.Create(quoteData);

            logger.LogInformation($"Quote {quoteData.Id} added to the DB");

            return Ok();
        }

        [HttpPost()]
        [ProducesResponseType(typeof(string), 200)]
        public async Task<IActionResult> DeleteQuote([FromBody] string quoteID)
        {
            logger.LogInformation($"Delete quote with id {quoteID}");

            await quotesService.Delete(quoteID);

            logger.LogInformation($"Quote {quoteID} deleted from the DB");

            return Ok();
        }

        [HttpGet()]
        [ProducesResponseType(typeof(string), 200)]
        public async Task<IActionResult> GetAll()
        {
            logger.LogInformation($"Getting all quotes from DB");

            List<Quote> allQuotes = await quotesService.GetAll();

            logger.LogInformation($"All Quotes selected from the DB");

            return Ok(allQuotes);
        }


        [HttpGet("{quoteID}")]
        [ProducesResponseType(typeof(string), 200)]
        public async Task<IActionResult> GetQuote(string quoteID)
        {
            var quote = await quotesService.GetQuote(quoteID);

            if (quote == null)
            {
                return NotFound($"There is no quote with this id {quoteID}");
            }

            return Ok(quote);
        }
    }
}
