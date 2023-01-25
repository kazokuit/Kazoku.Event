using Microsoft.WindowsAzure.Storage.Table;

namespace Website.Models
{
    public class Occupation : TableEntity
    {
        public string Id { get { return RowKey; } }
        public string Name { get { return PartitionKey; } }
    }
}
