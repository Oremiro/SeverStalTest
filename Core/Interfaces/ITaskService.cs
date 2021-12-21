using System.Threading.Tasks;
using DAL.Models.Requests;

namespace Core.Interfaces
{
    public interface ITaskService
    {
        Task CreateAsync(CreateStockRequest request);
    }
}