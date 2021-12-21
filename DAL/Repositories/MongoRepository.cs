using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DAL.Attributes;
using DAL.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using Task = System.Threading.Tasks.Task;

namespace DAL.Repositories
{
    public class MongoRepository<TBaseModel> : IMongoRepository<TBaseModel>
        where TBaseModel : IBaseModel
    {
        private readonly IMongoCollection<TBaseModel> _collection;

        public MongoRepository(IMongoDbSettings settings)
        {
            var database = new MongoClient(settings.ConnectionString).GetDatabase(settings.DatabaseName);
            _collection = database.GetCollection<TBaseModel>(GetCollectionName(typeof(TBaseModel)));
        }

        private protected string GetCollectionName(Type documentType)
        {
            return ((BsonCollectionAttribute) documentType.GetCustomAttributes(
                    typeof(BsonCollectionAttribute),
                    true)
                .FirstOrDefault())?.CollectionName;
        }

        public virtual IQueryable<TBaseModel> AsQueryable()
        {
            return _collection.AsQueryable();
        }

        public virtual IEnumerable<TBaseModel> FilterBy(
            Expression<Func<TBaseModel, bool>> filterExpression)
        {
            return _collection.Find(filterExpression).ToEnumerable();
        }

        public virtual IEnumerable<TProjected> FilterBy<TProjected>(
            Expression<Func<TBaseModel, bool>> filterExpression,
            Expression<Func<TBaseModel, TProjected>> projectionExpression)
        {
            return _collection.Find(filterExpression).Project(projectionExpression).ToEnumerable();
        }

        public virtual TBaseModel FindOne(Expression<Func<TBaseModel, bool>> filterExpression)
        {
            return _collection.Find(filterExpression).FirstOrDefault();
        }

        public virtual Task<TBaseModel> FindOneAsync(Expression<Func<TBaseModel, bool>> filterExpression)
        {
            return Task.Run(() => _collection.Find(filterExpression).FirstOrDefaultAsync());
        }

        public virtual TBaseModel FindById(string id)
        {
            var objectId = new ObjectId(id);
            var filter = Builders<TBaseModel>.Filter.Eq(doc => doc.Id, objectId);
            return _collection.Find(filter).SingleOrDefault();
        }

        public virtual Task<TBaseModel> FindByIdAsync(string id)
        {
            return Task.Run(() =>
            {
                var objectId = new ObjectId(id);
                var filter = Builders<TBaseModel>.Filter.Eq(doc => doc.Id, objectId);
                return _collection.Find(filter).SingleOrDefaultAsync();
            });
        }


        public virtual void InsertOne(TBaseModel document)
        {
            _collection.InsertOne(document);
        }

        public virtual Task InsertOneAsync(TBaseModel document)
        {
            return Task.Run(() => _collection.InsertOneAsync(document));
        }
        
        public virtual async Task InsertManyAsync(ICollection<TBaseModel> documents)
        {
            await _collection.InsertManyAsync(documents);
        }

        public virtual async Task ReplaceOneAsync(TBaseModel document)
        {
            var filter = Builders<TBaseModel>.Filter.Eq(doc => doc.Id, document.Id);
            await _collection.FindOneAndReplaceAsync(filter, document);
        }

        public Task DeleteOneAsync(Expression<Func<TBaseModel, bool>> filterExpression)
        {
            return Task.Run(() => _collection.FindOneAndDeleteAsync(filterExpression));
        }
        public Task DeleteByIdAsync(string id)
        {
            return Task.Run(() =>
            {
                var objectId = new ObjectId(id);
                var filter = Builders<TBaseModel>.Filter.Eq(doc => doc.Id, objectId);
                _collection.FindOneAndDeleteAsync(filter);
            });
        }

        public Task DeleteManyAsync(Expression<Func<TBaseModel, bool>> filterExpression)
        {
            return Task.Run(() => _collection.DeleteManyAsync(filterExpression));
        }
    }
}