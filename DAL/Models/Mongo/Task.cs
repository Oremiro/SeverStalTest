using DAL.Attributes;

namespace DAL.Models.Mongo
{
    [BsonCollection("Task")]
    public class Task: BaseMongoModel
    {
        public string MessageSource { get; set; }
        public string PlatformSource { get; set; }
        public int? StatusCode { get; set; }
    }
}