using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Core.Helpers;
using Core.Interfaces;
using DAL.Interfaces;
using DAL.Models.Internal;
using DAL.Models.Mongo;
using DAL.Models.Requests;
using DAL.Models.Responses;
using MongoDB.Bson;
using Task = System.Threading.Tasks.Task;


namespace Core.Services
{
    public class StoreService: IStoreService
    {
        private IMongoRepository<Store> _storeRepository { get; set; }
        private IMongoRepository<Stock> _stockRepository { get; set; }

        private IUriHelper _uriHelper { get; set; }
        public StoreService(IMongoRepository<Store> storeRepository, IMongoRepository<Stock> stockRepository, IUriHelper uriHelper)
        {
            this._storeRepository = storeRepository;
            this._stockRepository = stockRepository;
            this._uriHelper = uriHelper;
        }
        
        public async Task<Response<Store>> GetAsync(string id)
        {
            var store = await this._storeRepository.FindByIdAsync(id);
            
            var request = new Response<Store>
            {
                StatusCode = (HttpStatusCode) 0,
                Data = store,
                Succeeded = true,
                Errors = new string[]
                {
                },
                Message = null
            };
            
            return request;
        }

        public async Task<PagedResponse<List<Stock>>> GetRelatedStocks(PaginationFilter filter, string route, string id)
        {
            var store = await this._storeRepository.FindByIdAsync(id);
            var stocks = this._stockRepository.FilterBy(q => q.StoreId.Equals(store));
            var stocksData = stocks.Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToList();
            
            var totalRecords =  this._stockRepository
                .AsQueryable()
                .Count();
            
            var pagedReponse = await PaginationHelper.CreatePagedReponseAsync<Stock>(stocksData, filter, totalRecords, _uriHelper, route);

            return pagedReponse; 
        }

        public async Task<PagedResponse<List<Store>>> GetFilteredAsync(PaginationFilter filter, string route)
        {
            var stores = this._storeRepository
                .AsQueryable()
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToList();
            
            var totalRecords =  this._storeRepository
                .AsQueryable()
                .Count();
            
            var pagedReponse = await PaginationHelper.CreatePagedReponseAsync<Store>(stores, filter, totalRecords, _uriHelper, route);

            return pagedReponse;
        }

        public async Task<Response<Store>> CreateAsync(CreateStoreRequest storeRequest)
        {
            Store store = new Store
            {
                StoreName = storeRequest.StoreName,
                CountryCode = storeRequest.CountryCode,
                Email = storeRequest.Email,
                Manager = storeRequest.Manager,
                ManagerEmail = storeRequest.ManagerEmail
            };

            await this._storeRepository.InsertOneAsync(store);
            var request = new Response<Store>
            {
                StatusCode = (HttpStatusCode) 0,
                Data = store,
                Succeeded = true,
                Errors = new string[]
                {
                },
                Message = null
            };
            return request;
        }

        public async Task<Response<Store>> UpdateAsync(UpdateStoreRequest storeRequest)
        {
            Store store = new Store
            {
                StoreName = storeRequest.StoreName,
                CountryCode = storeRequest.CountryCode,
                Email = storeRequest.Email,
                Manager = storeRequest.Manager,
                ManagerEmail = storeRequest.ManagerEmail
            };

            await this._storeRepository.ReplaceOneAsync(store);
            var request = new Response<Store>
            {
                StatusCode = (HttpStatusCode) 0,
                Data = store,
                Succeeded = true,
                Errors = new string[]
                {
                },
                Message = null
            };
            return request;
        }

        public async Task DeleteAsync(string id)
        {
            await this._storeRepository.DeleteByIdAsync(id.ToString());
        }
    }
}