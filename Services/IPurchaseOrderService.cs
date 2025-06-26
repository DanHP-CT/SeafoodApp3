using SeafoodApp.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SeafoodApp.Services
{
    public interface IPurchaseOrderService
    {
        Task<IEnumerable<PurchaseOrder>> GetAllPurchaseOrdersAsync();
        Task<PurchaseOrder?> GetPurchaseOrderByIdAsync(int id);
        Task CreatePurchaseOrderAsync(PurchaseOrder purchaseOrder);
        Task UpdatePurchaseOrderAsync(PurchaseOrder purchaseOrder);
        Task DeletePurchaseOrderAsync(int id);
        Task<string> GeneratePurchaseOrderNumberAsync();
        Task<string> GenerateLotNumberAsync();
        Task AllocatePurchaseOrderAsync(int purchaseOrderId);
    }
}