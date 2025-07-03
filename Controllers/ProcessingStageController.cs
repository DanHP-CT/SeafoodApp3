using Microsoft.AspNetCore.Mvc;
using SeafoodApp.Models.Entities;
using SeafoodApp.Services;
using System.Threading.Tasks;

namespace SeafoodApp.Controllers
{
    public class ProcessingStageController : Controller
    {
        private readonly IProcessingStageService _processingStageService;

        public ProcessingStageController(IProcessingStageService processingStageService)
        {
            _processingStageService = processingStageService;
        }

        public async Task<IActionResult> Index()
        {
            var stages = await _processingStageService.GetAllProcessingStagesAsync();
            return View(stages);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProcessingStage model)
        {
            if (ModelState.IsValid)
            {
                await _processingStageService.CreateProcessingStageAsync(model);
                TempData["Success"] = "Tạo công đoạn chế biến thành công!";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var stage = await _processingStageService.GetProcessingStageByIdAsync(id);
            if (stage == null) return NotFound();
            return View(stage);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProcessingStage model)
        {
            if (id != model.Id || !ModelState.IsValid) return View(model);
            var stage = await _processingStageService.GetProcessingStageByIdAsync(id);
            if (stage == null) return NotFound();
            stage.StageName = model.StageName;
            stage.Description = model.Description;
            stage.InputMaterial = model.InputMaterial;
            stage.OutputProduct = model.OutputProduct;
            stage.StandardRate = model.StandardRate;
            await _processingStageService.UpdateProcessingStageAsync(stage);
            TempData["Success"] = "Cập nhật công đoạn chế biến thành công!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var stage = await _processingStageService.GetProcessingStageByIdAsync(id);
            if (stage == null) return NotFound();
            return View(stage);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _processingStageService.DeleteProcessingStageAsync(id);
            TempData["Success"] = "Xóa công đoạn chế biến thành công!";
            return RedirectToAction(nameof(Index));
        }
    }
}