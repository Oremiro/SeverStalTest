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
    public class StockController: BaseController
    {
        private IStockService _stockService { get; }
        private ITaskService _taskService { get; }
        public StockController(IStockService stockService, ITaskService taskService)
        {
            this._stockService = stockService;
            this._taskService = taskService;
        }

        [HttpGet]
        [Route("get/{id}")]
        public async Task<Response<Stock>> Get([FromRoute] string id)
        {
            if (id == null) throw new ArgumentNullException(nameof(id));
            
            return await this._stockService.GetAsync(id);
        }
        [HttpGet]
        [Route("get/filtered")]
        public async Task<PagedResponse<List<Stock>>> GetFiltered([FromQuery] PaginationFilter filter)
        {
            if (filter == null) throw new ArgumentNullException(nameof(filter));
            var route = Request.Path.Value;
            
            return await this._stockService.GetFilteredAsync(filter, route);
        }
        [HttpPost]
        [Route("create")]
        public async Task Create([FromBody] CreateStockRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            
            await this._taskService.CreateAsync(request);
        }
     
        
    }
}