using SeafoodApp.Models.Entities;
using SeafoodApp.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SeafoodApp.Services
{
    public class ProductWageRateService : IProductWageRateService
    {
        private readonly IProductWageRateRepository _repository;

        public ProductWageRateService(IProductWageRateRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ProductWageRate>> GetAllProductWageRatesAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<ProductWageRate?> GetProductWageRateByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task CreateProductWageRateAsync(ProductWageRate productWageRate)
        {
            await _repository.AddAsync(productWageRate);
        }

        public async Task UpdateProductWageRateAsync(ProductWageRate productWageRate)
        {
            await _repository.UpdateAsync(productWageRate);
        }

        public async Task DeleteProductWageRateAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<ProductWageRate>> GetByProcessingStageIdAsync(int processingStageId)
        {
            return await _repository.GetByProcessingStageIdAsync(processingStageId);
        }
    }
}