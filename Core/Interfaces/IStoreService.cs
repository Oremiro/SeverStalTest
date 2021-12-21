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
    public interface IStoreService
    {
        Task<Response<Store>> GetAsync(string id);
        Task<PagedResponse<List<Stock>>> GetRelatedStocks(PaginationFilter filter, string route, string id);

        Task<PagedResponse<List<Store>>> GetFilteredAsync(PaginationFilter filter, string route);
        Task<Response<Store>> CreateAsync(CreateStoreRequest storeRequest);
        Task<Response<Store>> UpdateAsync(UpdateStoreRequest request);
        Task DeleteAsync(string id);
    }
}