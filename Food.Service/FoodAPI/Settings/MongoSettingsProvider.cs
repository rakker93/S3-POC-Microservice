namespace FoodAPI.Settings
{
    public class MongoSettingsProvider
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string CollectionName { get; set; }
    }
}