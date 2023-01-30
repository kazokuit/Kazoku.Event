using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Website.Models.Configs;
using System.Diagnostics;
using Website.Models;
using Microsoft.Extensions.Options;

namespace Website.Services
{
    public class StorageService
    {
        private readonly CloudTableClient _client;
        private readonly IOptions<AzureOptions> _azure;

        public StorageService(IOptions<AzureOptions> azure)
        {
            _azure = azure;
            _client = CreateCloudTableClient();
            
        }
 
        private CloudTableClient CreateCloudTableClient()
        {
            // Get Storage Information
            var accountName = _azure.Value.AccountName;
            var accountKey = _azure.Value.AccountKey;
            var connectionString = _azure.Value.ConnectionString;

            // Set Auth
            //var creds = new StorageCredentials(accountName, accountKey);
            //var account = new CloudStorageAccount(creds, useHttps: true);
            var storageAccount = CloudStorageAccount.Parse(connectionString);

            // Connect to Storage
            return storageAccount.CreateCloudTableClient();
        }

        private async Task<CloudTable> GetTableAsync(string tableName)
        {
            CloudTable table = _client.GetTableReference(tableName);
            await table.CreateIfNotExistsAsync();
            return table;
        }

        public async Task<List<Event>> GetEventsAsync()
        {
            List<Event> events = new List<Event>();

            CloudTable eventsTable = await GetTableAsync("eventkazokutable");
            var eventsQuery = new TableQuery<Event>();

            TableContinuationToken? continuationToken = null;
            do
            {
                var queryResults = await eventsTable.ExecuteQuerySegmentedAsync(eventsQuery, continuationToken);

                Console.WriteLine(queryResults.Count());
                continuationToken = queryResults.ContinuationToken;
                events = queryResults.Results.ToList();
            } while (continuationToken != null);

            return events;
        }

        public async Task InsertEventAsync(Event currentEvent)
        {
            //TableOperation insertOperation = TableOperation.InsertOrReplace(currentEvent);
            //TableResult result = await table.ExecuteAsync(insertOperation);

            CloudTable table = await GetTableAsync("eventkazokutable");
            var op = TableOperation.Insert(currentEvent);
            await table.ExecuteAsync(op);
        }
    }
}
