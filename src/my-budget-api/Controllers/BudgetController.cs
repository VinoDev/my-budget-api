using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using my_budget_api.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace my_budget_api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BudgetController : ControllerBase
    {
        private readonly IDocumentClient _documentClient;
        readonly string databaseId;
        readonly string collectionId;
        public IConfiguration Configuration { get; }

        public BudgetController(IDocumentClient documentClient, IConfiguration configuration)
        {
            _documentClient = documentClient;
            Configuration = configuration;

            databaseId = Configuration["DatabaseId"];
            collectionId = "Budgets";

            BuildCollection().Wait();
        }

        private async Task BuildCollection()
        {
            await _documentClient.CreateDatabaseIfNotExistsAsync(new Database { Id = databaseId });
            await _documentClient.CreateDocumentCollectionIfNotExistsAsync(
                UriFactory.CreateDatabaseUri(databaseId),
                new DocumentCollection { Id = collectionId }
            );
        }

        [HttpGet]
        public IQueryable<Budget> Get()
        {
            return _documentClient.CreateDocumentQuery<Budget>(
                UriFactory.CreateDocumentCollectionUri(databaseId, collectionId),
                new FeedOptions { MaxItemCount = 10 }
            );
        }

        [HttpGet("{id}")]
        public IQueryable<Budget> Get(string id)
        {
            return _documentClient.CreateDocumentQuery<Budget>(
                UriFactory.CreateDocumentCollectionUri(databaseId, collectionId),
                new FeedOptions { MaxItemCount = 1 }
            ).Where((budget) => budget.Id == id);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Budget budget)
        {
            var response = await _documentClient.CreateDocumentAsync(
                UriFactory.CreateDocumentCollectionUri(databaseId, collectionId),
                budget
            );

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] Budget budget)
        {
            var response = await _documentClient.ReplaceDocumentAsync(
                UriFactory.CreateDocumentUri(databaseId, collectionId, id),
                budget
            );
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _documentClient.DeleteDocumentAsync(
                    UriFactory.CreateDocumentUri(databaseId, collectionId, id)
                );
                return Ok();

            }
            catch
            {
                return NoContent();
            }
        }
    }
}
