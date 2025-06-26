using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SeafoodApp.Data;
using SeafoodApp.Models.Entities;
using SeafoodApp.Models.ViewModels;
using SeafoodApp.Services;
using System.Linq;
using System.Threading.Tasks;

namespace SeafoodApp.Controllers
{
    public class PurchaseOrderController : Controller
    {
        private readonly IPurchaseOrderService _purchaseOrderService;
        private readonly ISupplierService _supplierService;
        private readonly AppDbContext _context;

        public PurchaseOrderController(
            IPurchaseOrderService purchaseOrderService,
            ISupplierService supplierService,
            AppDbContext context)
        {
            _purchaseOrderService = purchaseOrderService;
            _supplierService = supplierService;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var purchaseOrders = await _purchaseOrderService.GetAllPurchaseOrdersAsync();
            var viewModel = purchaseOrders.Select(po => new PurchaseOrderViewModel
            {
                Id = po.Id,
                PurchaseOrderNumber = po.PurchaseOrderNumber,
                CreatedDate = po.CreatedDate,
                SupplyDate = po.SupplyDate,
                SupplierId = po.SupplierId,
                SupplierName = po.Supplier.Name,
                Status = po.Status,
                TotalQuantity = po.TotalQuantity,
                TotalAmount = po.TotalAmount
            }).ToList();
            return View(viewModel);
        }

        public async Task<IActionResult> Details(int id)
        {
            var purchaseOrder = await _purchaseOrderService.GetPurchaseOrderByIdAsync(id);
            if (purchaseOrder == null)
            {
                return NotFound($"Không tìm thấy phiếu mua với ID: {id}");
            }

            var viewModel = new PurchaseOrderViewModel
            {
                Id = purchaseOrder.Id,
                PurchaseOrderNumber = purchaseOrder.PurchaseOrderNumber,
                CreatedDate = purchaseOrder.CreatedDate,
                SupplyDate = purchaseOrder.SupplyDate,
                SupplierId = purchaseOrder.SupplierId,
                SupplierName = purchaseOrder.Supplier?.Name ?? "Không có thông tin",
                Status = purchaseOrder.Status ?? "Chưa điều phối",
                TotalQuantity = purchaseOrder.TotalQuantity,
                TotalAmount = purchaseOrder.TotalAmount,
                Details = purchaseOrder.Details?.Select(d => new PurchaseOrderDetailViewModel
                {
                    Id = d.Id,
                    ProductName = d.ProductName ?? string.Empty,
                    Size = d.Size ?? string.Empty,
                    Price = d.Price,
                    Quantity = d.Quantity,
                    Amount = d.Amount
                })?.ToList() ?? new List<PurchaseOrderDetailViewModel>()
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Create()
        {
            var suppliers = await _supplierService.GetAllSuppliersAsync();
            ViewBag.Suppliers = new SelectList(suppliers, "Id", "Name");

            var model = new PurchaseOrderViewModel
            {
                PurchaseOrderNumber = await _purchaseOrderService.GeneratePurchaseOrderNumberAsync(),
                Details = new List<PurchaseOrderDetailViewModel> { new() }
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PurchaseOrderViewModel model)
        {
            if (ModelState.IsValid)
            {
                var purchaseOrder = new PurchaseOrder
                {
                    PurchaseOrderNumber = model.PurchaseOrderNumber,
                    CreatedDate = model.CreatedDate,
                    SupplyDate = model.SupplyDate,
                    SupplierId = model.SupplierId,
                    Status = model.Status,
                    Details = model.Details.Select(d => new PurchaseOrderDetail
                    {
                        ProductName = d.ProductName,
                        Size = d.Size,
                        Price = d.Price,
                        Quantity = d.Quantity,
                        Amount = d.Price * d.Quantity
                    }).ToList()
                };

                purchaseOrder.TotalQuantity = purchaseOrder.Details.Sum(d => d.Quantity);
                purchaseOrder.TotalAmount = purchaseOrder.Details.Sum(d => d.Amount);

                await _purchaseOrderService.CreatePurchaseOrderAsync(purchaseOrder);
                return RedirectToAction(nameof(Index));
            }

            var suppliers = await _supplierService.GetAllSuppliersAsync();
            ViewBag.Suppliers = new SelectList(suppliers, "Id", "Name", model.SupplierId);
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var purchaseOrder = await _purchaseOrderService.GetPurchaseOrderByIdAsync(id);
            if (purchaseOrder == null)
            {
                return NotFound();
            }

            var viewModel = new PurchaseOrderViewModel
            {
                Id = purchaseOrder.Id,
                PurchaseOrderNumber = purchaseOrder.PurchaseOrderNumber,
                CreatedDate = purchaseOrder.CreatedDate,
                SupplyDate = purchaseOrder.SupplyDate,
                SupplierId = purchaseOrder.SupplierId,
                SupplierName = purchaseOrder.Supplier.Name,
                Status = purchaseOrder.Status,
                TotalQuantity = purchaseOrder.TotalQuantity,
                TotalAmount = purchaseOrder.TotalAmount,
                Details = purchaseOrder.Details.Select(d => new PurchaseOrderDetailViewModel
                {
                    Id = d.Id,
                    ProductName = d.ProductName,
                    Size = d.Size,
                    Price = d.Price,
                    Quantity = d.Quantity,
                    Amount = d.Amount
                }).ToList()
            };

            var suppliers = await _supplierService.GetAllSuppliersAsync();
            ViewBag.Suppliers = new SelectList(suppliers, "Id", "Name", viewModel.SupplierId);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PurchaseOrderViewModel model)
        {
            if (ModelState.IsValid)
            {
                var purchaseOrder = await _purchaseOrderService.GetPurchaseOrderByIdAsync(model.Id);
                if (purchaseOrder == null)
                {
                    return NotFound();
                }

                purchaseOrder.PurchaseOrderNumber = model.PurchaseOrderNumber;
                purchaseOrder.CreatedDate = model.CreatedDate;
                purchaseOrder.SupplyDate = model.SupplyDate;
                purchaseOrder.SupplierId = model.SupplierId;
                purchaseOrder.Status = model.Status;

                // Cập nhật chi tiết
                purchaseOrder.Details.Clear();
                purchaseOrder.Details = model.Details.Select(d => new PurchaseOrderDetail
                {
                    Id = d.Id,
                    ProductName = d.ProductName,
                    Size = d.Size,
                    Price = d.Price,
                    Quantity = d.Quantity,
                    Amount = d.Price * d.Quantity
                }).ToList();

                purchaseOrder.TotalQuantity = purchaseOrder.Details.Sum(d => d.Quantity);
                purchaseOrder.TotalAmount = purchaseOrder.Details.Sum(d => d.Amount);

                await _purchaseOrderService.UpdatePurchaseOrderAsync(purchaseOrder);
                return RedirectToAction(nameof(Index));
            }

            var suppliers = await _supplierService.GetAllSuppliersAsync();
            ViewBag.Suppliers = new SelectList(suppliers, "Id", "Name", model.SupplierId);
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var purchaseOrder = await _purchaseOrderService.GetPurchaseOrderByIdAsync(id);
            if (purchaseOrder == null)
            {
                return NotFound();
            }

            var viewModel = new PurchaseOrderViewModel
            {
                Id = purchaseOrder.Id,
                PurchaseOrderNumber = purchaseOrder.PurchaseOrderNumber,
                CreatedDate = purchaseOrder.CreatedDate,
                SupplyDate = purchaseOrder.SupplyDate,
                SupplierId = purchaseOrder.SupplierId,
                SupplierName = purchaseOrder.Supplier.Name,
                Status = purchaseOrder.Status,
                TotalQuantity = purchaseOrder.TotalQuantity,
                TotalAmount = purchaseOrder.TotalAmount
            };

            return View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _purchaseOrderService.DeletePurchaseOrderAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Redirect(int id) // Đổi tên từ Allocate thành Redirect
        {
            return RedirectToAction("Index", "Allocation", new { purchaseOrderId = id });
        }
    }
}