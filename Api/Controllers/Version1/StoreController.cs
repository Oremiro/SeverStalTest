using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Interfaces;
using DAL.Models.Internal;
using DAL.Models.Mongo;
using DAL.Models.Requests;
using DAL.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Task = System.Threading.Tasks.Task;

namespace Api.Controllers.Version1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class StoreController : BaseController
    {
        private IStoreService _storeService { get; }
        private IStockService _stockService { get; }

        public StoreController(IStoreService storeService, IStockService stockService)
        {
            this._storeService = storeService;
            this._stockService = stockService;
        }

        [HttpGet]
        [Route("get/{id}")]
        public async Task<Response<Store>> Get([FromRoute] string id)
        {
            if (id == null) throw new ArgumentNullException(nameof(id));

            return await this._storeService.GetAsync(id);
        }
        
        [HttpGet]
        [Route("get/{id}/stocks")]
        public async Task<Response<Store>> GetRelatedStocks([FromRoute] string id)
        {
            if (id == null) throw new ArgumentNullException(nameof(id));

            return await this._storeService.GetAsync(id);
        }

        [HttpGet]
        [Route("get/filtered")]
        public async Task<PagedResponse<List<Store>>> GetFiltered([FromQuery] PaginationFilter filter)
        {
            if (filter == null) throw new ArgumentNullException(nameof(filter));
            var route = Request.Path.Value;
            return await this._storeService.GetFilteredAsync(filter, route);
        }

        [HttpPost]
        [Route("create")]
        public async Task<Response<Store>> Create([FromBody] CreateStoreRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            return await this._storeService.CreateAsync(request);
        }

        [HttpPut]
        [Route("update")]
        public async Task<Response<Store>> Update([FromBody] UpdateStoreRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            return await this._storeService.UpdateAsync(request);
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task Delete([FromRoute] string id)
        {
            if (id == null) throw new ArgumentNullException(nameof(id));

            await this._storeService.DeleteAsync(id);
        }
    }
}