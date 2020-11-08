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
    public class ExpenseController : ControllerBase
    {
        private readonly IDocumentClient _documentClient;
        readonly string databaseId;
        readonly string collectionId;
        public IConfiguration Configuration { get; }

        public ExpenseController(IDocumentClient documentClient, IConfiguration configuration)
        {
            _documentClient = documentClient;
            Configuration = configuration;

            databaseId = Configuration["DatabaseId"];
            collectionId = "Expenses";

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
        public IQueryable<Expense> Get()
        {
            return _documentClient.CreateDocumentQuery<Expense>(
                UriFactory.CreateDocumentCollectionUri(databaseId, collectionId),
                new FeedOptions { MaxItemCount = 10 }
            );
        }

        [HttpGet("{id}")]
        public IQueryable<Expense> Get(string id)
        {
            return _documentClient.CreateDocumentQuery<Expense>(
                UriFactory.CreateDocumentCollectionUri(databaseId, collectionId),
                new FeedOptions { MaxItemCount = 1 }
            ).Where((expense)=> expense.Id == id);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Expense expense)
        {
            var response = await _documentClient.CreateDocumentAsync(
                UriFactory.CreateDocumentCollectionUri(databaseId, collectionId),
                expense
            );

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put(string id, [FromBody] Expense expense)
        {
            var response = await _documentClient.ReplaceDocumentAsync(
                UriFactory.CreateDocumentUri(databaseId, collectionId, id),
                expense
            );

            return Ok();
        }
    }
}
