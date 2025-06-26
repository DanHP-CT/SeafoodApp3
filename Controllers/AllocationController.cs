using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SeafoodApp.Models.Entities;
using SeafoodApp.Models.ViewModels;
using SeafoodApp.Services;
using System.Threading.Tasks;

namespace SeafoodApp.Controllers
{
    public class AllocationController : Controller
    {
        private readonly IPurchaseOrderService _purchaseOrderService;
        private readonly IAllocationService _allocationService;

        public AllocationController(
            IPurchaseOrderService purchaseOrderService,
            IAllocationService allocationService)
        {
            _purchaseOrderService = purchaseOrderService;
            _allocationService = allocationService;
        }

        public async Task<IActionResult> Index(int purchaseOrderId)
        {
            var viewModel = await _allocationService.GetAllocationsByPurchaseOrderIdAsync(purchaseOrderId);
            return View(viewModel);
        }

        public async Task<IActionResult> Allocate(int id)
        {
            var allocation = await _allocationService.GetAllocationByIdAsync(id);
            if (allocation.AllocatedQuantity == 0 && string.IsNullOrEmpty(allocation.Size))
            {
                // Nếu chưa có allocation, lấy thông tin từ Lot
                var lot = await _allocationService.GetLotByIdAsync(id);
                if (lot != null)
                {
                    allocation.LotId = lot.Id;
                    allocation.LotNumber = lot.LotNumber;
                    allocation.ProductName = lot.ProductName;
                    allocation.Size = lot.ProductName.Split('-').LastOrDefault() ?? "N/A";
                    allocation.AllocatedQuantity = 0; // Khởi tạo SL điều phối
                }
            }
            allocation.ProductionOrders = await _allocationService.GetActiveProductionOrdersAsync();
            return View(allocation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Allocate(AllocationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var allocation = new Allocation
                {
                    LotId = model.LotId,
                    Size = model.Size,
                    AllocatedQuantity = model.AllocatedQuantity,
                    ProductionOrderId = model.ProductionOrderId
                };
                await _allocationService.CreateAllocationAsync(allocation);
                return RedirectToAction("Index", new { purchaseOrderId = model.LotId });
            }
            model.ProductionOrders = await _allocationService.GetActiveProductionOrdersAsync();
            return View(model);
        }

        public async Task<IActionResult> AllocateAll(int purchaseOrderId)
        {
            var viewModel = await _allocationService.GetAllocationsByPurchaseOrderIdAsync(purchaseOrderId);
            await Task.WhenAll(viewModel.Allocations.Select(async a => a.ProductionOrders = await _allocationService.GetActiveProductionOrdersAsync()));
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AllocateAll(AllocationListViewModel model)
        {
            if (ModelState.IsValid)
            {
                foreach (var allocation in model.Allocations)
                {
                    if (allocation.AllocatedQuantity > 0)
                    {
                        var entity = new Allocation
                        {
                            LotId = allocation.LotId,
                            Size = allocation.Size,
                            AllocatedQuantity = allocation.AllocatedQuantity,
                            ProductionOrderId = allocation.ProductionOrderId
                        };
                        await _allocationService.CreateAllocationAsync(entity);
                    }
                }
                return RedirectToAction("Index", new { purchaseOrderId = model.PurchaseOrderId });
            }
            await Task.WhenAll(model.Allocations.Select(async a => a.ProductionOrders = await _allocationService.GetActiveProductionOrdersAsync()));
            return View(model);
        }
    }
}