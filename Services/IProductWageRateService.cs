using SeafoodApp.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SeafoodApp.Services
{
    public interface IProductWageRateService
    {
        Task<IEnumerable<ProductWageRate>> GetAllProductWageRatesAsync();
        Task<ProductWageRate?> GetProductWageRateByIdAsync(int id);
        Task CreateProductWageRateAsync(ProductWageRate productWageRate);
        Task UpdateProductWageRateAsync(ProductWageRate productWageRate);
        Task DeleteProductWageRateAsync(int id);
        Task<IEnumerable<ProductWageRate>> GetByProcessingStageIdAsync(int processingStageId);
    }
}