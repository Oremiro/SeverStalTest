using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Core.Helpers;
using Core.Interfaces;
using DAL.Interfaces;
using DAL.Models.Internal;
using DAL.Models.Mongo;
using DAL.Models.Responses;
using MongoDB.Bson;
using Task = System.Threading.Tasks.Task;

namespace Core.Services
{
    public class StockService: IStockService
    {
        private IMongoRepository<Stock> _stockRepository { get; set; }
        private IUriHelper _uriHelper { get; set; }
        public StockService(IMongoRepository<Stock> stockRepository, IUriHelper uriHelper)
        {
            this._stockRepository = stockRepository;
            this._uriHelper = uriHelper;
        }
        public async Task<Response<Stock>> GetAsync(string id)
        {
            var stock = await this._stockRepository.FindByIdAsync(id);
            
            var request = new Response<Stock>
            {
                StatusCode = (HttpStatusCode) 0,
                Data = stock,
                Succeeded = true,
                Errors = new string[]
                {
                },
                Message = null
            };
            
            return request;
        }

        public async Task<PagedResponse<List<Stock>>> GetFilteredAsync(PaginationFilter filter, string route)
        {
            var stores = this._stockRepository
                .AsQueryable()
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToList();
            
            var totalRecords =  this._stockRepository
                .AsQueryable()
                .Count();
            
            var pagedReponse = await PaginationHelper.CreatePagedReponseAsync<Stock>(stores, filter, totalRecords, _uriHelper, route);

            return pagedReponse;
        }
    }
}