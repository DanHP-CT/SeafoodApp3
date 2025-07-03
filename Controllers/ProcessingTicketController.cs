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
    public class ProcessingTicketController : Controller
    {
        private readonly IProcessingTicketService _processingTicketService;
        private readonly IInventoryService _inventoryService;
        private readonly IProcessingStageService _processingStageService;
        private readonly AppDbContext _context;

        public ProcessingTicketController(
            IProcessingTicketService processingTicketService,
            IInventoryService inventoryService,
            IProcessingStageService processingStageService,
            AppDbContext context)
        {
            _processingTicketService = processingTicketService;
            _inventoryService = inventoryService;
            _processingStageService = processingStageService;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var processingTickets = await _processingTicketService.GetAllProcessingTicketsAsync();
            var viewModel = processingTickets.Select(pt => new ProcessingTicketViewModel
            {
                Id = pt.Id,
                ProcessingTicketNumber = pt.ProcessingTicketNumber,
                ProductionOrderId = pt.ProductionOrderId,
                ProductionOrderNumber = pt.ProductionOrder.ProductionOrderNumber,
                ProcessingStageId = pt.ProcessingStageId,
                ProcessingStageName = pt.ProcessingStage.StageName,
                ProcessingDate = pt.ProcessingDate,
                Status = pt.Status
            });
            return View(viewModel);
        }

        public async Task<IActionResult> Details(int id)
        {
            var processingTicket = await _processingTicketService.GetProcessingTicketByIdAsync(id);
            if (processingTicket == null)
            {
                return NotFound();
            }

            var viewModel = new ProcessingTicketViewModel
            {
                Id = processingTicket.Id,
                ProcessingTicketNumber = processingTicket.ProcessingTicketNumber,
                ProductionOrderId = processingTicket.ProductionOrderId,
                ProductionOrderNumber = processingTicket.ProductionOrder.ProductionOrderNumber,
                ProcessingStageId = processingTicket.ProcessingStageId,
                ProcessingStageName = processingTicket.ProcessingStage.StageName,
                ProcessingDate = processingTicket.ProcessingDate,
                Status = processingTicket.Status,
                Details = processingTicket.Details.Select(d => new ProcessingTicketDetailViewModel
                {
                    Id = d.Id,
                    AllocationId = d.AllocationId,
                    LotNumber = d.Allocation.Lot.LotNumber,
                    ProductName = d.ProductName,
                    Size = d.Size,
                    ProcessedQuantity = d.ProcessedQuantity,
                    OutputProductName = d.OutputProductName,
                    OutputSize = d.OutputSize,
                    OutputQuantity = d.OutputQuantity
                }).ToList()
            };

            ViewBag.LaborCost = await _processingTicketService.CalculateLaborCostAsync(id);
            return View(viewModel);
        }

        public async Task<IActionResult> Create()
        {
            var model = new ProcessingTicketViewModel
            {
                ProcessingTicketNumber = await _processingTicketService.GenerateProcessingTicketNumberAsync(),
                Details = new List<ProcessingTicketDetailViewModel> { new() }
            };
            model.ProductionOrders = await _processingTicketService.GetActiveProductionOrdersAsync() ?? new List<SelectListItem>();
            model.ProcessingStages = (await _processingStageService.GetAllActiveAsync())
                .Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.StageName })
                .ToList() ?? new List<SelectListItem>();
            ViewBag.Allocations = await _processingTicketService.GetAvailableAllocationsAsync(model.ProductionOrderId) ?? new List<SelectListItem>();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProcessingTicketViewModel model)
        {
            if (ModelState.IsValid)
            {
                var processingTicket = new ProcessingTicket
                {
                    ProcessingTicketNumber = model.ProcessingTicketNumber,
                    ProductionOrderId = model.ProductionOrderId,
                    ProcessingStageId = model.ProcessingStageId,
                    ProcessingDate = model.ProcessingDate,
                    Status = model.Status,
                    Details = model.Details.Select(d => new ProcessingTicketDetail
                    {
                        AllocationId = d.AllocationId,
                        ProductName = d.ProductName,
                        Size = d.Size,
                        ProcessedQuantity = d.ProcessedQuantity,
                        OutputProductName = d.OutputProductName,
                        OutputSize = d.OutputSize,
                        OutputQuantity = d.OutputQuantity
                    }).ToList()
                };

                await _processingTicketService.CreateProcessingTicketAsync(processingTicket);
                TempData["Success"] = "Tạo phiếu chế biến thành công!";
                return RedirectToAction(nameof(Index));
            }
            model.ProductionOrders = await _processingTicketService.GetActiveProductionOrdersAsync() ?? new List<SelectListItem>();
            model.ProcessingStages = (await _processingStageService.GetAllActiveAsync())
                .Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.StageName })
                .ToList() ?? new List<SelectListItem>();
            ViewBag.Allocations = await _processingTicketService.GetAvailableAllocationsAsync(model.ProductionOrderId) ?? new List<SelectListItem>();
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var processingTicket = await _processingTicketService.GetProcessingTicketByIdAsync(id);
            if (processingTicket == null)
            {
                return NotFound();
            }

            var viewModel = new ProcessingTicketViewModel
            {
                Id = processingTicket.Id,
                ProcessingTicketNumber = processingTicket.ProcessingTicketNumber,
                ProductionOrderId = processingTicket.ProductionOrderId,
                ProductionOrderNumber = processingTicket.ProductionOrder.ProductionOrderNumber,
                ProcessingStageId = processingTicket.ProcessingStageId,
                ProcessingStageName = processingTicket.ProcessingStage.StageName,
                ProcessingDate = processingTicket.ProcessingDate,
                Status = processingTicket.Status,
                Details = processingTicket.Details.Select(d => new ProcessingTicketDetailViewModel
                {
                    Id = d.Id,
                    AllocationId = d.AllocationId,
                    LotNumber = d.Allocation.Lot.LotNumber,
                    ProductName = d.ProductName,
                    Size = d.Size,
                    ProcessedQuantity = d.ProcessedQuantity,
                    OutputProductName = d.OutputProductName,
                    OutputSize = d.OutputSize,
                    OutputQuantity = d.OutputQuantity
                }).ToList()
            };

            viewModel.ProductionOrders = await _processingTicketService.GetActiveProductionOrdersAsync() ?? new List<SelectListItem>();
            viewModel.ProcessingStages = (await _processingStageService.GetAllActiveAsync())
                .Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.StageName })
                .ToList() ?? new List<SelectListItem>();
            ViewBag.Allocations = await _processingTicketService.GetAvailableAllocationsAsync(viewModel.ProductionOrderId) ?? new List<SelectListItem>();
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProcessingTicketViewModel model)
        {
            if (id != model.Id || !ModelState.IsValid)
            {
                model.ProductionOrders = await _processingTicketService.GetActiveProductionOrdersAsync() ?? new List<SelectListItem>();
                model.ProcessingStages = (await _processingStageService.GetAllActiveAsync())
                    .Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.StageName })
                    .ToList() ?? new List<SelectListItem>();
                ViewBag.Allocations = await _processingTicketService.GetAvailableAllocationsAsync(model.ProductionOrderId) ?? new List<SelectListItem>();
                return View(model);
            }

            var processingTicket = await _processingTicketService.GetProcessingTicketByIdAsync(id);
            if (processingTicket == null)
            {
                return NotFound();
            }

            processingTicket.ProcessingTicketNumber = model.ProcessingTicketNumber;
            processingTicket.ProductionOrderId = model.ProductionOrderId;
            processingTicket.ProcessingStageId = model.ProcessingStageId;
            processingTicket.ProcessingDate = model.ProcessingDate;
            processingTicket.Status = model.Status;
            processingTicket.Details.Clear();
            processingTicket.Details = model.Details.Select(d => new ProcessingTicketDetail
            {
                Id = d.Id,
                AllocationId = d.AllocationId,
                ProductName = d.ProductName,
                Size = d.Size,
                ProcessedQuantity = d.ProcessedQuantity,
                OutputProductName = d.OutputProductName,
                OutputSize = d.OutputSize,
                OutputQuantity = d.OutputQuantity
            }).ToList();

            await _processingTicketService.UpdateProcessingTicketAsync(processingTicket);
            TempData["Success"] = "Cập nhật phiếu chế biến thành công!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var processingTicket = await _processingTicketService.GetProcessingTicketByIdAsync(id);
            if (processingTicket == null)
            {
                return NotFound();
            }

            var viewModel = new ProcessingTicketViewModel
            {
                Id = processingTicket.Id,
                ProcessingTicketNumber = processingTicket.ProcessingTicketNumber,
                ProductionOrderId = processingTicket.ProductionOrderId,
                ProductionOrderNumber = processingTicket.ProductionOrder.ProductionOrderNumber,
                ProcessingStageId = processingTicket.ProcessingStageId,
                ProcessingStageName = processingTicket.ProcessingStage.StageName,
                ProcessingDate = processingTicket.ProcessingDate,
                Status = processingTicket.Status
            };

            return View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _processingTicketService.DeleteProcessingTicketAsync(id);
            TempData["Success"] = "Xóa phiếu chế biến thành công!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Complete(int id)
        {
            var processingTicket = await _processingTicketService.GetProcessingTicketByIdAsync(id);
            if (processingTicket == null)
            {
                return NotFound();
            }

            var viewModel = new ProcessingTicketViewModel
            {
                Id = processingTicket.Id,
                ProcessingTicketNumber = processingTicket.ProcessingTicketNumber,
                ProductionOrderId = processingTicket.ProductionOrderId,
                ProductionOrderNumber = processingTicket.ProductionOrder.ProductionOrderNumber,
                ProcessingStageId = processingTicket.ProcessingStageId,
                ProcessingStageName = processingTicket.ProcessingStage.StageName,
                ProcessingDate = processingTicket.ProcessingDate,
                Status = processingTicket.Status,
                Details = processingTicket.Details.Select(d => new ProcessingTicketDetailViewModel
                {
                    Id = d.Id,
                    AllocationId = d.AllocationId,
                    LotNumber = d.Allocation.Lot.LotNumber,
                    ProductName = d.ProductName,
                    Size = d.Size,
                    ProcessedQuantity = d.ProcessedQuantity,
                    OutputProductName = d.OutputProductName,
                    OutputSize = d.OutputSize,
                    OutputQuantity = d.OutputQuantity
                }).ToList()
            };

            viewModel.ProductionOrders = await _processingTicketService.GetActiveProductionOrdersAsync() ?? new List<SelectListItem>();
            viewModel.ProcessingStages = (await _processingStageService.GetAllActiveAsync())
                .Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.StageName })
                .ToList() ?? new List<SelectListItem>();
            ViewBag.Allocations = await _processingTicketService.GetAvailableAllocationsAsync(viewModel.ProductionOrderId) ?? new List<SelectListItem>();
            ViewBag.LaborCost = await _processingTicketService.CalculateLaborCostAsync(id);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Complete(int id, ProcessingTicketViewModel model, string action)
        {
            if (!ModelState.IsValid)
            {
                model.ProductionOrders = await _processingTicketService.GetActiveProductionOrdersAsync() ?? new List<SelectListItem>();
                model.ProcessingStages = (await _processingStageService.GetAllActiveAsync())
                    .Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.StageName })
                    .ToList() ?? new List<SelectListItem>();
                ViewBag.Allocations = await _processingTicketService.GetAvailableAllocationsAsync(model.ProductionOrderId) ?? new List<SelectListItem>();
                TempData["Error"] = "Dữ liệu không hợp lệ!";
                return View(model);
            }

            try
            {
                await _processingTicketService.CompleteProcessingTicketAsync(id, model.Details, action, _inventoryService);
                TempData["Success"] = "Hoàn thành phiếu chế biến thành công!";

                if (action == "Chuyển công đoạn tiếp theo")
                {
                    var newTicket = new ProcessingTicketViewModel
                    {
                        ProductionOrderId = model.ProductionOrderId,
                        ProcessingDate = DateTime.Now,
                        Details = model.Details.Select(d => new ProcessingTicketDetailViewModel
                        {
                            ProductName = d.OutputProductName,
                            Size = d.OutputSize,
                            ProcessedQuantity = d.OutputQuantity
                        }).ToList()
                    };
                    return RedirectToAction("Create", newTicket);
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi khi hoàn thành phiếu chế biến: {ex.Message}";
                model.ProductionOrders = await _processingTicketService.GetActiveProductionOrdersAsync() ?? new List<SelectListItem>();
                model.ProcessingStages = (await _processingStageService.GetAllActiveAsync())
                    .Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.StageName })
                    .ToList() ?? new List<SelectListItem>();
                ViewBag.Allocations = await _processingTicketService.GetAvailableAllocationsAsync(model.ProductionOrderId) ?? new List<SelectListItem>();
                return View(model);
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetAllocationDetails(int allocationId)
        {
            var allocation = await _context.Allocations
                .Include(a => a.Lot)
                .FirstOrDefaultAsync(a => a.Id == allocationId && !a.IsDeleted);
            if (allocation == null)
            {
                return Json(new { success = false });
            }

            return Json(new
            {
                success = true,
                productName = allocation.Lot.ProductName,
                size = allocation.Size
            });
        }
    }
}