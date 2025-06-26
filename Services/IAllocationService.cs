using Microsoft.AspNetCore.Mvc.Rendering;
using SeafoodApp.Models.Entities;
using SeafoodApp.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SeafoodApp.Services
{
    public interface IAllocationService
    {
        Task<AllocationListViewModel> GetAllocationsByPurchaseOrderIdAsync(int purchaseOrderId);
        Task<AllocationViewModel> GetAllocationByIdAsync(int id);
        Task CreateAllocationAsync(Allocation allocation);
        Task UpdateAllocationAsync(Allocation allocation);
        Task DeleteAllocationAsync(int id);
        Task<List<SelectListItem>> GetActiveProductionOrdersAsync();
        Task<Lot> GetLotByIdAsync(int id); // Thêm phương thức mới
    }
}