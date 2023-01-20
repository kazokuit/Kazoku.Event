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
        private readonly AzureOptions _azure;

        public StorageService(IOptions<AzureOptions> azure)
        {
            _client = CreateCloudTableClient();
            _azure = azure.Value;
        }
 
        private CloudTableClient CreateCloudTableClient()
        {
            // Get Storage Information
            var accountName = _azure.AccountName;
            var accountKey = _azure.AccountKey;

            // Set Auth
            var creds = new StorageCredentials(accountName, accountKey);
            var account = new CloudStorageAccount(creds, useHttps: true);

            // Connect to Storage
            return account.CreateCloudTableClient();
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

            CloudTable eventsTable = await GetTableAsync("Events");
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
    }
}
