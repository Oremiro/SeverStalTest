namespace DAL.Interfaces
{
    public interface IMongoDbSettings
    {
        string ConnectionString { get; }
        string DatabaseName { get; }
        
        public string JsonCollectionName { get; }
        public string TelegramCollectionName { get; }
    }
}