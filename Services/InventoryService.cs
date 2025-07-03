using SeafoodApp.Models.Entities;
using SeafoodApp.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SeafoodApp.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryRepository _repository;

        public InventoryService(IInventoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Inventory>> GetAllInventoriesAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Inventory?> GetInventoryByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id); // Trả về Inventory? để khớp với Task<Inventory?>
        }

        public async Task CreateInventoryAsync(Inventory inventory)
        {
            inventory.StockQuantity = inventory.InputQuantity - inventory.OutputQuantity;
            await _repository.AddAsync(inventory);
        }

        public async Task UpdateInventoryAsync(Inventory inventory)
        {
            inventory.StockQuantity = inventory.InputQuantity - inventory.OutputQuantity;
            await _repository.UpdateAsync(inventory);
        }

        public async Task DeleteInventoryAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Inventory>> SearchInventoriesAsync(string lotNumber, string productName, string productType, string size)
        {
            return await _repository.SearchAsync(lotNumber, productName, productType, size);
        }
    }
}