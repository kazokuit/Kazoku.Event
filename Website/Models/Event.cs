using Microsoft.WindowsAzure.Storage.Table;

namespace Website.Models
{
    public class Event : TableEntity
    {
        public Event()
        {

        }

        public string Id { get { return PartitionKey; } }
        public string Name { get { return RowKey; } }
        public string Description { get; set; } = string.Empty; 
        public string Location { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public DateTime Deadline { get; set; }
        public int MaxParticipants { get; set; }
        public bool Interrnal { get; set; } = false;
    }
}
