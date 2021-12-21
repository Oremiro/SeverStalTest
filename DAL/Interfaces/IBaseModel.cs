using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Interfaces
{
    public interface IBaseModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        ObjectId Id { get; set; }

        DateTime CreatedAt { get; }
    }
}