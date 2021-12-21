using DAL.Interfaces;

namespace DAL.Models.Internal
{
    public class MongoDbSettings: IMongoDbSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string JsonCollectionName { get; set; }
        public string TelegramCollectionName { get; set; }
    }
}