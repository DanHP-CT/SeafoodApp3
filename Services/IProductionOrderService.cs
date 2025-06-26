using SeafoodApp.Models.Entities;
using SeafoodApp.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SeafoodApp.Services
{
    public interface IProductionOrderService
    {
        Task<IEnumerable<ProductionOrder>> GetAllProductionOrdersAsync();
        Task<ProductionOrder?> GetProductionOrderByIdAsync(int id);
        Task CreateProductionOrderAsync(ProductionOrder productionOrder);
        Task UpdateProductionOrderAsync(ProductionOrder productionOrder);
        Task DeleteProductionOrderAsync(int id);
        Task<string> GenerateProductionOrderNumberAsync();
    }
}