using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IMongoRepository<TBaseModel> where TBaseModel : IBaseModel
    {
        IQueryable<TBaseModel> AsQueryable();

        IEnumerable<TBaseModel> FilterBy(
            Expression<Func<TBaseModel, bool>> filterExpression);

        IEnumerable<TProjected> FilterBy<TProjected>(
            Expression<Func<TBaseModel, bool>> filterExpression,
            Expression<Func<TBaseModel, TProjected>> projectionExpression);
        
        Task<TBaseModel> FindOneAsync(Expression<Func<TBaseModel, bool>> filterExpression);

        Task<TBaseModel> FindByIdAsync(string id);
        
        Task InsertOneAsync(TBaseModel document);
        
        Task InsertManyAsync(ICollection<TBaseModel> documents);

        Task ReplaceOneAsync(TBaseModel document);
        
        Task DeleteOneAsync(Expression<Func<TBaseModel, bool>> filterExpression);
        
        Task DeleteByIdAsync(string id);

        Task DeleteManyAsync(Expression<Func<TBaseModel, bool>> filterExpression);
    }
}