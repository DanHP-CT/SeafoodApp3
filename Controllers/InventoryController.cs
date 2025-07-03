using Microsoft.AspNetCore.Mvc;
using SeafoodApp.Models.Entities;
using SeafoodApp.Models.ViewModels;
using SeafoodApp.Services;
using System.Linq;
using System.Threading.Tasks;

namespace SeafoodApp.Controllers
{
    public class InventoryController : Controller
    {
        private readonly IInventoryService _inventoryService;

        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        public async Task<IActionResult> Index(string lotNumber = "", string productName = "", string productType = "", string size = "")
        {
            var inventories = await _inventoryService.SearchInventoriesAsync(lotNumber, productName, productType, size);
            var model = new InventoryFilterViewModel
            {
                LotNumber = lotNumber,
                ProductName = productName,
                ProductType = productType,
                Size = size,
                Inventories = inventories.Select(i => new InventoryViewModel
                {
                    Id = i.Id,
                    LotNumber = i.LotNumber,
                    ProductName = i.ProductName,
                    ProductType = i.ProductType,
                    Size = i.Size,
                    InputQuantity = i.InputQuantity,
                    OutputQuantity = i.OutputQuantity,
                    StockQuantity = i.StockQuantity,
                    InputDate = i.InputDate,
                    OutputDate = i.OutputDate,
                    Note = i.Note
                }).ToList()
            };
            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InventoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var inventory = new Inventory
                {
                    LotNumber = model.LotNumber,
                    ProductName = model.ProductName,
                    ProductType = model.ProductType,
                    Size = model.Size,
                    InputQuantity = model.InputQuantity,
                    OutputQuantity = model.OutputQuantity,
                    InputDate = model.InputDate,
                    OutputDate = model.OutputDate,
                    Note = model.Note
                };
                await _inventoryService.CreateInventoryAsync(inventory);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var inventory = await _inventoryService.GetInventoryByIdAsync(id);
            if (inventory == null) return NotFound();
            return View(new InventoryViewModel
            {
                Id = inventory.Id,
                LotNumber = inventory.LotNumber,
                ProductName = inventory.ProductName,
                ProductType = inventory.ProductType,
                Size = inventory.Size,
                InputQuantity = inventory.InputQuantity,
                OutputQuantity = inventory.OutputQuantity,
                InputDate = inventory.InputDate,
                OutputDate = inventory.OutputDate,
                Note = inventory.Note
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, InventoryViewModel model)
        {
            if (id != model.Id || !ModelState.IsValid) return View(model);
            var inventory = await _inventoryService.GetInventoryByIdAsync(id);
            if (inventory == null) return NotFound();
            inventory.LotNumber = model.LotNumber;
            inventory.ProductName = model.ProductName;
            inventory.ProductType = model.ProductType;
            inventory.Size = model.Size;
            inventory.InputQuantity = model.InputQuantity;
            inventory.OutputQuantity = model.OutputQuantity;
            inventory.InputDate = model.InputDate;
            inventory.OutputDate = model.OutputDate;
            inventory.Note = model.Note;
            await _inventoryService.UpdateInventoryAsync(inventory);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var inventory = await _inventoryService.GetInventoryByIdAsync(id);
            if (inventory == null) return NotFound();
            return View(new InventoryViewModel
            {
                Id = inventory.Id,
                LotNumber = inventory.LotNumber,
                ProductName = inventory.ProductName,
                ProductType = inventory.ProductType,
                Size = inventory.Size,
                InputQuantity = inventory.InputQuantity,
                OutputQuantity = inventory.OutputQuantity,
                InputDate = inventory.InputDate,
                OutputDate = inventory.OutputDate,
                Note = inventory.Note
            });
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _inventoryService.DeleteInventoryAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}