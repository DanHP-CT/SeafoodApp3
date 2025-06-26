using Microsoft.AspNetCore.Mvc;
using SeafoodApp.Models.ViewModels;
using SeafoodApp.Services;
using SeafoodApp.Models.Entities;
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
            var viewModel = productionOrders.Select(po => new ProductionOrderViewModel
            {
                Id = po.Id,
                ProductionOrderNumber = po.ProductionOrderNumber,
                ContractNumber = po.ContractNumber,
                CustomerName = po.CustomerName,
                PackagingSupplyDate = po.PackagingSupplyDate,
                CompletionDate = po.CompletionDate,
                Status = po.Status
            });
            return View(viewModel);
        }

        public async Task<IActionResult> Details(int id)
        {
            var productionOrder = await _productionOrderService.GetProductionOrderByIdAsync(id);
            if (productionOrder == null)
            {
                return NotFound();
            }

            var viewModel = new ProductionOrderViewModel
            {
                Id = productionOrder.Id,
                ProductionOrderNumber = productionOrder.ProductionOrderNumber,
                ContractNumber = productionOrder.ContractNumber,
                CustomerName = productionOrder.CustomerName,
                PackagingSupplyDate = productionOrder.PackagingSupplyDate,
                CompletionDate = productionOrder.CompletionDate,
                Status = productionOrder.Status,
                Details = productionOrder.Details.Select(d => new ProductionOrderDetailViewModel
                {
                    Id = d.Id,
                    ProductName = d.ProductName,
                    Size = d.Size,
                    Packaging = d.Packaging,
                    Quantity = d.Quantity,
                    Note = d.Note
                }).ToList()
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Create()
        {
            var model = new ProductionOrderViewModel
            {
                ProductionOrderNumber = await _productionOrderService.GenerateProductionOrderNumberAsync(),
                Details = new List<ProductionOrderDetailViewModel> { new() }
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductionOrderViewModel model)
        {
            if (ModelState.IsValid)
            {
                var productionOrder = new ProductionOrder
                {
                    ProductionOrderNumber = model.ProductionOrderNumber,
                    ContractNumber = model.ContractNumber,
                    CustomerName = model.CustomerName,
                    PackagingSupplyDate = model.PackagingSupplyDate,
                    CompletionDate = model.CompletionDate,
                    Status = model.Status,
                    Details = model.Details.Select(d => new ProductionOrderDetail
                    {
                        ProductName = d.ProductName,
                        Size = d.Size,
                        Packaging = d.Packaging,
                        Quantity = d.Quantity,
                        Note = d.Note
                    }).ToList()
                };

                await _productionOrderService.CreateProductionOrderAsync(productionOrder);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var productionOrder = await _productionOrderService.GetProductionOrderByIdAsync(id);
            if (productionOrder == null)
            {
                return NotFound();
            }

            var viewModel = new ProductionOrderViewModel
            {
                Id = productionOrder.Id,
                ProductionOrderNumber = productionOrder.ProductionOrderNumber,
                ContractNumber = productionOrder.ContractNumber,
                CustomerName = productionOrder.CustomerName,
                PackagingSupplyDate = productionOrder.PackagingSupplyDate,
                CompletionDate = productionOrder.CompletionDate,
                Status = productionOrder.Status,
                Details = productionOrder.Details.Select(d => new ProductionOrderDetailViewModel
                {
                    Id = d.Id,
                    ProductName = d.ProductName,
                    Size = d.Size,
                    Packaging = d.Packaging,
                    Quantity = d.Quantity,
                    Note = d.Note
                }).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductionOrderViewModel model)
        {
            if (ModelState.IsValid)
            {
                var productionOrder = await _productionOrderService.GetProductionOrderByIdAsync(model.Id);
                if (productionOrder == null)
                {
                    return NotFound();
                }

                productionOrder.ProductionOrderNumber = model.ProductionOrderNumber;
                productionOrder.ContractNumber = model.ContractNumber;
                productionOrder.CustomerName = model.CustomerName;
                productionOrder.PackagingSupplyDate = model.PackagingSupplyDate;
                productionOrder.CompletionDate = model.CompletionDate;
                productionOrder.Status = model.Status;

                productionOrder.Details.Clear();
                productionOrder.Details = model.Details.Select(d => new ProductionOrderDetail
                {
                    Id = d.Id,
                    ProductName = d.ProductName,
                    Size = d.Size,
                    Packaging = d.Packaging,
                    Quantity = d.Quantity,
                    Note = d.Note
                }).ToList();

                await _productionOrderService.UpdateProductionOrderAsync(productionOrder);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var productionOrder = await _productionOrderService.GetProductionOrderByIdAsync(id);
            if (productionOrder == null)
            {
                return NotFound();
            }

            var viewModel = new ProductionOrderViewModel
            {
                Id = productionOrder.Id,
                ProductionOrderNumber = productionOrder.ProductionOrderNumber,
                ContractNumber = productionOrder.ContractNumber,
                CustomerName = productionOrder.CustomerName,
                PackagingSupplyDate = productionOrder.PackagingSupplyDate,
                CompletionDate = productionOrder.CompletionDate,
                Status = productionOrder.Status
            };

            return View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _productionOrderService.DeleteProductionOrderAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}