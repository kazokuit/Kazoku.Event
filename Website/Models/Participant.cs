using Microsoft.WindowsAzure.Storage.Table;

namespace Website.Models
{
    public class Participant : TableEntity
    {
        public string Id { get { return RowKey; } }
        public string EventId { get { return PartitionKey; } }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Info { get; set; }
    }
}