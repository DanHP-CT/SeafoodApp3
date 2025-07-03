using Microsoft.AspNetCore.Mvc;
using SeafoodApp.Models.Entities;
using SeafoodApp.Services;
using System.Threading.Tasks;

namespace SeafoodApp.Controllers
{
    public class ProductWageRateController : Controller
    {
        private readonly IProductWageRateService _productWageRateService;
        private readonly IProcessingStageService _processingStageService;

        public ProductWageRateController(
            IProductWageRateService productWageRateService,
            IProcessingStageService processingStageService)
        {
            _productWageRateService = productWageRateService;
            _processingStageService = processingStageService;
        }

        public async Task<IActionResult> Index()
        {
            var wageRates = await _productWageRateService.GetAllProductWageRatesAsync();
            return View(wageRates);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.ProcessingStages = await _processingStageService.GetAllActiveAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductWageRate model)
        {
            if (ModelState.IsValid)
            {
                await _productWageRateService.CreateProductWageRateAsync(model);
                TempData["Success"] = "Tạo đơn giá lương thành công!";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.ProcessingStages = await _processingStageService.GetAllActiveAsync();
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var wageRate = await _productWageRateService.GetProductWageRateByIdAsync(id);
            if (wageRate == null) return NotFound();
            ViewBag.ProcessingStages = await _processingStageService.GetAllActiveAsync();
            return View(wageRate);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductWageRate model)
        {
            if (id != model.Id || !ModelState.IsValid)
            {
                ViewBag.ProcessingStages = await _processingStageService.GetAllActiveAsync();
                return View(model);
            }
            var wageRate = await _productWageRateService.GetProductWageRateByIdAsync(id);
            if (wageRate == null) return NotFound();
            wageRate.ProcessingStageId = model.ProcessingStageId;
            wageRate.WageRate = model.WageRate;
            await _productWageRateService.UpdateProductWageRateAsync(wageRate);
            TempData["Success"] = "Cập nhật đơn giá lương thành công!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var wageRate = await _productWageRateService.GetProductWageRateByIdAsync(id);
            if (wageRate == null) return NotFound();
            return View(wageRate);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _productWageRateService.DeleteProductWageRateAsync(id);
            TempData["Success"] = "Xóa đơn giá lương thành công!";
            return RedirectToAction(nameof(Index));
        }
    }
}