using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Models.Internal;
using DAL.Models.Mongo;
using DAL.Models.Requests;
using DAL.Models.Responses;
using MongoDB.Bson;
using Task = System.Threading.Tasks.Task;

namespace Core.Interfaces
{
    public interface IStockService
    {
        Task<Response<Stock>> GetAsync(string id);
        Task<PagedResponse<List<Stock>>> GetFilteredAsync(PaginationFilter filter, string route);
    }
}