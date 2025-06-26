using Microsoft.EntityFrameworkCore;
using SeafoodApp.Data;
using SeafoodApp.Models.Entities;
using SeafoodApp.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SeafoodApp.Services
{
    public class ProductionOrderService : IProductionOrderService
    {
        private readonly IRepository<ProductionOrder> _productionOrderRepository;
        private readonly AppDbContext _context;

        public ProductionOrderService(
            IRepository<ProductionOrder> productionOrderRepository,
            AppDbContext context)
        {
            _productionOrderRepository = productionOrderRepository;
            _context = context;
        }

        public async Task<IEnumerable<ProductionOrder>> GetAllProductionOrdersAsync()
        {
            return await _context.ProductionOrders
                .Include(po => po.Details)
                .Include(po => po.Allocations)
                .ToListAsync();
        }

        public async Task<ProductionOrder?> GetProductionOrderByIdAsync(int id)
        {
            return await _context.ProductionOrders
                .Include(po => po.Details)
                .Include(po => po.Allocations)
                .FirstOrDefaultAsync(po => po.Id == id);
        }

        public async Task CreateProductionOrderAsync(ProductionOrder productionOrder)
        {
            productionOrder.ProductionOrderNumber = await GenerateProductionOrderNumberAsync();
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

        public async Task<string> GenerateProductionOrderNumberAsync()
        {
            var year = DateTime.Now.ToString("yy"); // Sử dụng năm 25 cho 2025
            var lastOrder = await _context.ProductionOrders
                .OrderByDescending(po => po.ProductionOrderNumber)
                .FirstOrDefaultAsync();

            int nextNumber = lastOrder == null ? 1 : int.Parse(lastOrder.ProductionOrderNumber.Split('-')[2]) + 1;
            return $"LSX-{year}-{nextNumber:D3}";
        }
    }
}