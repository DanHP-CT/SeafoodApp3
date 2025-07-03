using Microsoft.EntityFrameworkCore;
using SeafoodApp.Data;
using SeafoodApp.Models.Entities;
using SeafoodApp.Models.ViewModels;
using SeafoodApp.Repositories;
using SeafoodApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SeafoodApp.Services
{
    public class ProductionOrderService : IProductionOrderService
    {
        private readonly IRepository<ProductionOrder> _productionOrderRepository;
        private readonly IRepository<ProductionOrderDetail> _productionOrderDetailRepository;
        private readonly IRepository<Allocation> _allocationRepository;
        private readonly IRepository<ProcessingTicket> _processingTicketRepository;
        private readonly IRepository<ProcessingTicketDetail> _processingTicketDetailRepository;
        private readonly IRepository<Inventory> _inventoryRepository;
        private readonly IProcessingTicketService _processingTicketService;
        private readonly AppDbContext _context;

        public ProductionOrderService(
            IRepository<ProductionOrder> productionOrderRepository,
            IRepository<ProductionOrderDetail> productionOrderDetailRepository,
            IRepository<Allocation> allocationRepository,
            IRepository<ProcessingTicket> processingTicketRepository,
            IRepository<ProcessingTicketDetail> processingTicketDetailRepository,
            IRepository<Inventory> inventoryRepository,
            IProcessingTicketService processingTicketService,
            AppDbContext context)
        {
            _productionOrderRepository = productionOrderRepository;
            _productionOrderDetailRepository = productionOrderDetailRepository;
            _allocationRepository = allocationRepository;
            _processingTicketRepository = processingTicketRepository;
            _processingTicketDetailRepository = processingTicketDetailRepository;
            _inventoryRepository = inventoryRepository;
            _processingTicketService = processingTicketService;
            _context = context;
        }

        public async Task<IEnumerable<ProductionOrder>> GetAllProductionOrdersAsync()
        {
            return await _context.ProductionOrders
                .Include(po => po.Details)
                .Where(po => !po.IsDeleted)
                .ToListAsync();
        }

        public async Task<ProductionOrder?> GetProductionOrderByIdAsync(int id)
        {
            return await _context.ProductionOrders
                .Include(po => po.Details)
                .FirstOrDefaultAsync(po => po.Id == id && !po.IsDeleted);
        }

        public async Task CreateProductionOrderAsync(ProductionOrder productionOrder)
        {
            await _productionOrderRepository.AddAsync(productionOrder);
        }

        public async Task UpdateProductionOrderAsync(ProductionOrder productionOrder)
        {
            await _productionOrderRepository.UpdateAsync(productionOrder);
        }

        public async Task DeleteProductionOrderAsync(int id)
        {
            await _productionOrderRepository.DeleteAsync(id);
        }

        public async Task<List<SelectListItem>> GetActiveSuppliersAsync()
        {
            return await _context.Suppliers
                .Where(s => !s.IsDeleted)
                .Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.Name
                }).ToListAsync();
        }

        public async Task<ProductionOrderDetailViewModel> GetProductionOrderDetailAsync(int id)
        {
            var productionOrder = await GetProductionOrderByIdAsync(id);
            if (productionOrder == null) throw new Exception("Không tìm thấy lệnh sản xuất");

            var viewModel = new ProductionOrderDetailViewModel
            {
                ProductionOrderId = productionOrder.Id,
                ProductionOrderNumber = productionOrder.ProductionOrderNumber
            };

            // Bảng 1: Nguyên liệu từ Phiếu mua nguyên liệu
            var allocations = await _context.Allocations
                .Include(a => a.Lot)
                .Include(a => a.PurchaseOrderDetail)
                .Where(a => a.ProductionOrderId == id && !a.IsDeleted)
                .ToListAsync();
            viewModel.PurchaseMaterials = allocations.Select(a => new PurchaseMaterialViewModel
            {
                Id = a.Id,
                ProductName = a.Lot.ProductName,
                Size = a.Size,
                Quantity = a.Lot.InputQuantity - a.AllocatedQuantity,
                LotNumber = a.Lot.LotNumber,
                UsedQuantity = 0,
                IsUsed = false
            }).ToList();

            // Bảng 2: Nguyên liệu từ Phiếu chế biến trước
            var completedTickets = await _context.ProcessingTickets
                .Include(pt => pt.Details)
                .Where(pt => pt.ProductionOrderId == id && pt.Status == "Hoàn thành" && !pt.IsDeleted)
                .ToListAsync();
            viewModel.ProcessedMaterials = completedTickets
                .SelectMany(pt => pt.Details)
                .Select(d => new ProcessedMaterialViewModel
                {
                    Id = d.Id,
                    ProductName = d.OutputProductName,
                    Size = d.OutputSize,
                    Quantity = d.OutputQuantity,
                    LotNumber = $"{pt.ProcessingTicketNumber}-{d.Id}", // pt là hợp lệ từ SelectMany
                    UsedQuantity = 0,
                    IsUsed = false
                }).ToList();

            // Bảng 3: Bán thành phẩm trong kho
            var inventoryItems = await _context.Inventories
                .Where(i => !i.IsDeleted && (i.ProductType == "BTP" || i.ProductType == "TP"))
                .ToListAsync();
            viewModel.InventoryItems = inventoryItems.Select(i => new InventoryItemViewModel
            {
                Id = i.Id,
                ProductName = i.ProductName,
                Size = i.Size,
                Quantity = i.InputQuantity - i.OutputQuantity,
                LotNumber = i.LotNumber,
                UsedQuantity = 0,
                IsUsed = false
            }).ToList();

            return viewModel;
        }

        public async Task CreateProcessingTicketFromProductionOrderAsync(int productionOrderId, ProductionOrderDetailViewModel model)
        {
            var processingTicket = new ProcessingTicket
            {
                ProcessingTicketNumber = await _processingTicketService.GenerateProcessingTicketNumberAsync(),
                ProductionOrderId = productionOrderId,
                ProcessingDate = DateTime.Now,
                Status = "Đang chế biến",
                Details = new List<ProcessingTicketDetail>()
            };

            // Thêm nguyên liệu từ Bảng 1
            foreach (var item in model.PurchaseMaterials.Where(pm => pm.IsUsed && pm.UsedQuantity > 0))
            {
                processingTicket.Details.Add(new ProcessingTicketDetail
                {
                    AllocationId = item.Id,
                    ProductName = item.ProductName,
                    Size = item.Size,
                    ProcessedQuantity = item.UsedQuantity,
                    OutputProductName = item.ProductName,
                    OutputSize = item.Size,
                    OutputQuantity = item.UsedQuantity
                });
            }

            // Thêm nguyên liệu từ Bảng 2
            foreach (var item in model.ProcessedMaterials.Where(pm => pm.IsUsed && pm.UsedQuantity > 0))
            {
                processingTicket.Details.Add(new ProcessingTicketDetail
                {
                    ProductName = item.ProductName,
                    Size = item.Size,
                    ProcessedQuantity = item.UsedQuantity,
                    OutputProductName = item.ProductName,
                    OutputSize = item.Size,
                    OutputQuantity = item.UsedQuantity
                });
            }

            // Thêm bán thành phẩm từ Bảng 3
            foreach (var item in model.InventoryItems.Where(pm => pm.IsUsed && pm.UsedQuantity > 0))
            {
                processingTicket.Details.Add(new ProcessingTicketDetail
                {
                    ProductName = item.ProductName,
                    Size = item.Size,
                    ProcessedQuantity = item.UsedQuantity,
                    OutputProductName = item.ProductName,
                    OutputSize = item.Size,
                    OutputQuantity = item.UsedQuantity
                });
            }

            await _processingTicketService.CreateProcessingTicketAsync(processingTicket);
        }
    }
}