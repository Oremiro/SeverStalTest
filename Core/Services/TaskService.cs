using Core.Interfaces;
using Core.Producers;
using DAL.Models.Mongo;
using DAL.Models.Requests;
using Task = System.Threading.Tasks.Task;

namespace Core.Services
{
    public class TaskService: ITaskService
    {
        /*
         * Возможно лучше создавать таски в бд, чтобы понимать их состояние в системе.
         */
        
        public async Task CreateAsync(CreateStockRequest request)
        {
            Stock stock = new Stock
            {
                StoreId = request.StoreId,
                Backstore = request.Backstore,
                FrontStore = request.FrontStore,
                ShoppingWindow = request.ShoppingWindow,
                StockAccuracy = request.StockAccuracy,
                OnFloorAvaillability = request.OnFloorAvaillability,
                MeanAgeDays = request.MeanAgeDays
            };

            await StockProducer.ProduceAsync(stock);
        }
    }
}