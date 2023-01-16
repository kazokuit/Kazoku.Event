namespace Website.Models.Configs
{
    public class AzureOptions
    {
        public const string Key = "Azure";
        public string AccountName { get; set; } = string.Empty;
        public string AccountKey { get; set; } = string.Empty;
        public string ConnectionString { get; set; } = string.Empty;
    }
}
