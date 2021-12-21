using DAL.Attributes;

namespace DAL.Models.Mongo
{
    [BsonCollection("Store")]
    public class Store: BaseMongoModel
    {
        public string StoreName { get; set; }
        public string CountryCode { get; set; }
        public string Email { get; set; }
        public string Manager { get; set; }
        public string ManagerEmail { get; set; }
    }
}