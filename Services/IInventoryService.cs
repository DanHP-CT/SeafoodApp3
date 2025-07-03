using SeafoodApp.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SeafoodApp.Services
{
    public interface IInventoryService
    {
        Task<IEnumerable<Inventory>> GetAllInventoriesAsync();
        Task<Inventory?> GetInventoryByIdAsync(int id);
        Task CreateInventoryAsync(Inventory inventory);
        Task UpdateInventoryAsync(Inventory inventory);
        Task DeleteInventoryAsync(int id);
        Task<IEnumerable<Inventory>> SearchInventoriesAsync(string lotNumber, string productName, string productType, string size);
    }
}