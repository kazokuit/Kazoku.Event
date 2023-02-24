using Microsoft.WindowsAzure.Storage.Table;

namespace Website.Models
{
    public class Participant : TableEntity
    {
        public string Id
        {
            get => RowKey;
            set => RowKey = value;
        }
        public string EventId { get => PartitionKey;
            set => PartitionKey = value;
        }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Info { get; set; }
    }
}