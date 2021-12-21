using DAL.Attributes;
using MongoDB.Bson;

namespace DAL.Models.Mongo
{
    [BsonCollection("Stock")]
    public class Stock: BaseMongoModel
    {
        public string StoreId { get; set; }
        public decimal? Backstore { get; set; }
        public decimal? FrontStore { get; set; }
        public decimal? ShoppingWindow { get; set; }
        public decimal? StockAccuracy { get; set; }
        public decimal? OnFloorAvaillability { get; set; }
        public decimal? MeanAgeDays { get; set; }
    }
}