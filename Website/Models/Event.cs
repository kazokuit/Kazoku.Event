using Microsoft.WindowsAzure.Storage.Table;

namespace Website.Models
{
    public class Event : TableEntity
    {
        public Event()
        {

        }

        public string Id { get => PartitionKey; set => PartitionKey = value; }
        public string Name { get => RowKey; set => RowKey = value; }
        public string Description { get; set; } = string.Empty; 
        public string Location { get; set; } = string.Empty;
        public DateTime Date { get; set; } = DateTime.Now;
        public DateTime Deadline { get; set; } = DateTime.Now.AddDays(1);
        public int MaxParticipants { get; set; }
        public bool Interrnal { get; set; } = false;
    }
}
