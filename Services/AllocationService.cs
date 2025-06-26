using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SeafoodApp.Data;
using SeafoodApp.Models.Entities;
using SeafoodApp.Models.ViewModels;
using SeafoodApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeafoodApp.Services
{
    public class AllocationService : IAllocationService
    {
        private readonly IRepository<Allocation> _allocationRepository;
        private readonly IRepository<Lot> _lotRepository;
        private readonly IRepository<PurchaseOrder> _purchaseOrderRepository;
        private readonly AppDbContext _context;

        public AllocationService(
            IRepository<Allocation> allocationRepository,
            IRepository<Lot> lotRepository,
            IRepository<PurchaseOrder> purchaseOrderRepository,
            AppDbContext context)
        {
            _allocationRepository = allocationRepository;
            _lotRepository = lotRepository;
            _purchaseOrderRepository = purchaseOrderRepository;
            _context = context;
        }

        public async Task<AllocationListViewModel> GetAllocationsByPurchaseOrderIdAsync(int purchaseOrderId)
        {
            var purchaseOrder = await _purchaseOrderRepository.GetByIdAsync(purchaseOrderId);
            if (purchaseOrder == null) return new AllocationListViewModel();

            var details = await _context.PurchaseOrderDetails
                .Where(d => d.PurchaseOrderId == purchaseOrderId && !d.IsDeleted)
                .ToListAsync();

            var lots = await _context.Lots
                .Where(l => l.PurchaseOrderId == purchaseOrderId && !l.IsDeleted)
                .ToListAsync();

            var allocations = await _context.Allocations
                .Include(a => a.Lot)
                .Where(a => lots.Select(l => l.Id).Contains(a.LotId) && !a.IsDeleted)
                .ToListAsync();

            var viewModel = new AllocationListViewModel
            {
                PurchaseOrderId = purchaseOrderId,
                PurchaseOrderNumber = purchaseOrder.PurchaseOrderNumber,
                Allocations = details.Select(d =>
                {
                    var lot = lots.FirstOrDefault(l => l.PurchaseOrderId == purchaseOrderId);
                    var allocated = allocations
                        .Where(a => a.Lot.PurchaseOrderId == purchaseOrderId && a.Size == d.Size)
                        .Sum(a => a.AllocatedQuantity);
                    return new AllocationViewModel
                    {
                        LotId = lot?.Id ?? 0,
                        LotNumber = lot?.LotNumber ?? "N/A",
                        ProductName = d.ProductName,
                        Size = d.Size,
                        AllocatedQuantity = allocated,
                        RemainingQuantity = d.Quantity - allocated,
                        ProductionOrderId = 0
                    };
                }).ToList(),
                TotalInputQuantity = details.Sum(d => d.Quantity),
                TotalAllocatedQuantity = allocations.Sum(a => a.AllocatedQuantity),
                RemainingQuantity = details.Sum(d => d.Quantity) - allocations.Sum(a => a.AllocatedQuantity)
            };

            return viewModel;
        }

        public async Task<AllocationViewModel> GetAllocationByIdAsync(int id)
        {
            var lot = await GetLotByIdAsync(id);
            if (lot == null) return new AllocationViewModel();

            var allocations = await _context.Allocations
                .Where(a => a.LotId == id && !a.IsDeleted)
                .ToListAsync();
            var allocatedQuantity = allocations.Sum(a => a.AllocatedQuantity);
            var remainingQuantity = lot.InputQuantity - allocatedQuantity;

            return new AllocationViewModel
            {
                Id = allocations.FirstOrDefault()?.Id ?? 0,
                LotId = lot.Id,
                LotNumber = lot.LotNumber,
                ProductName = lot.ProductName,
                Size = allocations.FirstOrDefault()?.Size ?? lot.ProductName.Split('-').LastOrDefault() ?? "N/A",
                AllocatedQuantity = allocatedQuantity,
                RemainingQuantity = remainingQuantity,
                ProductionOrderId = allocations.FirstOrDefault()?.ProductionOrderId ?? 0,
                ProductionOrders = await GetActiveProductionOrdersAsync()
            };
        }

        public async Task CreateAllocationAsync(Allocation allocation)
        {
            var lot = await _lotRepository.GetByIdAsync(allocation.LotId);
            if (lot != null && allocation.AllocatedQuantity <= (lot.InputQuantity - lot.AllocatedQuantity))
            {
                lot.AllocatedQuantity += allocation.AllocatedQuantity;
                await _lotRepository.UpdateAsync(lot);
                await _allocationRepository.AddAsync(allocation);
            }
        }

        public async Task UpdateAllocationAsync(Allocation allocation)
        {
            await _allocationRepository.UpdateAsync(allocation);
        }

        public async Task DeleteAllocationAsync(int id)
        {
            await _allocationRepository.DeleteAsync(id);
        }

        public async Task<List<SelectListItem>> GetActiveProductionOrdersAsync()
        {
            var productionOrders = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "LSX-25-001 (Khách A)" },
                new SelectListItem { Value = "2", Text = "LSX-25-002 (Khách B)" }
            };
            return await Task.FromResult(productionOrders);
        }

        public async Task<Lot> GetLotByIdAsync(int id)
        {
            var lot = await _lotRepository.GetByIdAsync(id);
            return lot ?? new Lot { Id = 0, InputQuantity = 0, AllocatedQuantity = 0 }; // Trả về Lot mặc định nếu null
        }
    }
}