using Microsoft.EntityFrameworkCore;
using SeafoodApp.Data;
using SeafoodApp.Models.Entities;
using SeafoodApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeafoodApp.Services
{
    public class PurchaseOrderService : IPurchaseOrderService
    {
        private readonly IRepository<PurchaseOrder> _purchaseOrderRepository;
        private readonly IRepository<Lot> _lotRepository;
        private readonly AppDbContext _context;

        public PurchaseOrderService(
            IRepository<PurchaseOrder> purchaseOrderRepository,
            IRepository<Lot> lotRepository,
            AppDbContext context)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
            _lotRepository = lotRepository;
            _context = context;
        }

        public async Task<IEnumerable<PurchaseOrder>> GetAllPurchaseOrdersAsync()
        {
            return await _context.PurchaseOrders
                .Include(po => po.Supplier)
                .Include(po => po.Details)
                .ToListAsync();
        }

        public async Task<PurchaseOrder?> GetPurchaseOrderByIdAsync(int id)
        {
            return await _context.PurchaseOrders
                .Include(po => po.Supplier)
                .Include(po => po.Details)
                .FirstOrDefaultAsync(po => po.Id == id);
        }

        public async Task CreatePurchaseOrderAsync(PurchaseOrder purchaseOrder)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await _purchaseOrderRepository.AddAsync(purchaseOrder);

                // Tạo lô nguyên liệu
                var lot = new Lot
                {
                    LotNumber = await GenerateLotNumberAsync(),
                    PurchaseOrderId = purchaseOrder.Id,
                    ProductName = purchaseOrder.Details.FirstOrDefault()?.ProductName ?? "Unknown",
                    InputQuantity = purchaseOrder.TotalQuantity,
                    AllocatedQuantity = 0
                };
                await _lotRepository.AddAsync(lot);

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task UpdatePurchaseOrderAsync(PurchaseOrder purchaseOrder)
        {
            await _purchaseOrderRepository.UpdateAsync(purchaseOrder);
        }

        public async Task DeletePurchaseOrderAsync(int id)
        {
            await _purchaseOrderRepository.DeleteAsync(id);
        }

        public async Task<string> GeneratePurchaseOrderNumberAsync()
        {
            var year = DateTime.Now.ToString("yy"); // Sử dụng năm 25 cho 2025
            var lastOrder = await _context.PurchaseOrders
                .OrderByDescending(po => po.PurchaseOrderNumber)
                .FirstOrDefaultAsync();

            int nextNumber = lastOrder == null ? 1 : int.Parse(lastOrder.PurchaseOrderNumber.Split('-')[2]) + 1;
            return $"PM-{year}-{nextNumber:D6}";
        }

        public async Task<string> GenerateLotNumberAsync()
        {
            var year = DateTime.Now.ToString("yy"); // Sử dụng năm 25 cho 2025
            var lastLot = await _context.Lots
                .OrderByDescending(l => l.LotNumber)
                .FirstOrDefaultAsync();

            int nextNumber = lastLot == null ? 1 : int.Parse(lastLot.LotNumber.Split('-')[2]) + 1;
            return $"LOT-{year}-{nextNumber:D6}";
        }

        public async Task AllocatePurchaseOrderAsync(int purchaseOrderId)
        {
            var purchaseOrder = await GetPurchaseOrderByIdAsync(purchaseOrderId);
            if (purchaseOrder != null)
            {
                purchaseOrder.Status = "Đã điều phối";
                await _purchaseOrderRepository.UpdateAsync(purchaseOrder);
            }
        }
    }
}