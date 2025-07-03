using Microsoft.AspNetCore.Mvc;
using SeafoodApp.Models.ViewModels;
using SeafoodApp.Services;
using System.Threading.Tasks;

namespace SeafoodApp.Controllers
{
    public class ProductionOrderController : Controller
    {
        private readonly IProductionOrderService _productionOrderService;

        public ProductionOrderController(IProductionOrderService productionOrderService)
        {
            _productionOrderService = productionOrderService;
        }

        public async Task<IActionResult> Index()
        {
            var productionOrders = await _productionOrderService.GetAllProductionOrdersAsync();
            return View(productionOrders);
        }

        public async Task<IActionResult> Details(int id)
        {
            var model = await _productionOrderService.GetProductionOrderDetailAsync(id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProcessingTicket(int productionOrderId, ProductionOrderDetailViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _productionOrderService.CreateProcessingTicketFromProductionOrderAsync(productionOrderId, model);
                TempData["Success"] = "Tạo Phiếu chế biến thành công!";
                return RedirectToAction("Index");
            }
            return View("Details", model);
        }

        // Các action khác (Create, Edit, Delete) giữ nguyên như trước
    }
}