namespace Website.Models.Configs
{
    public class AzureOptions
    {
        public const string Key = "AzureOptions";
        public string AccountName { get; set; }
        public string AccountKey { get; set; }
        public string ConnectionString { get; set; }
    }
}
