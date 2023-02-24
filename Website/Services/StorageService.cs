using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage;
using Website.Models.Configs;
using Website.Models;
using Microsoft.Extensions.Options;

namespace Website.Services
{
    public class StorageService
    {
        private const string EventsTable = "Events";
        private const string ParticipantsTable = "Participants";
            
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

            // Parsing connection string 
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

            CloudTable eventsTable = await GetTableAsync(EventsTable);
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

        public async Task<List<Participant>> GetParticipantsAsync()
        {
            List<Participant> participants = new List<Participant>();

            CloudTable participantsTable = await GetTableAsync(ParticipantsTable);
            var participantsQuery = new TableQuery<Participant>();

            TableContinuationToken? continuationToken = null;
            do
            {
                var queryResults = await participantsTable.ExecuteQuerySegmentedAsync(participantsQuery, continuationToken);

                Console.WriteLine(queryResults.Count());
                continuationToken = queryResults.ContinuationToken;
                participants = queryResults.Results.ToList();
            } while (continuationToken != null);

            return participants;
        }
        
        public async Task InsertParticipantAsync(Participant participant)
        {
            CloudTable table = await GetTableAsync(ParticipantsTable);
            var op = TableOperation.Insert(participant);
            await table.ExecuteAsync(op);
            
        }

        public async Task DeleteParticipantAsync(Participant participant)
        {
            CloudTable table = await GetTableAsync(ParticipantsTable);
            var op = TableOperation.Delete(participant);
            await table.ExecuteAsync(op);
        }

        public async Task InsertEventAsync(Event currentEvent)
        {
            CloudTable table = await GetTableAsync(EventsTable);
            var op = TableOperation.Insert(currentEvent);
            await table.ExecuteAsync(op);
            
        }

        public async Task DeleteEventAsync(Event currentEvent)
        {
            CloudTable table = await GetTableAsync(EventsTable);
            var op = TableOperation.Delete(currentEvent);
            await table.ExecuteAsync(op);
        }
    }
}
