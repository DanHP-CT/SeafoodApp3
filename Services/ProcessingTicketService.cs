using Microsoft.EntityFrameworkCore;
using SeafoodApp.Data;
using SeafoodApp.Models.Entities;
using SeafoodApp.Models.ViewModels;
using SeafoodApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SeafoodApp.Services
{
    public class ProcessingTicketService : IProcessingTicketService
    {
        private readonly IRepository<ProcessingTicket> _processingTicketRepository;
        private readonly IRepository<ProcessingTicketDetail> _processingTicketDetailRepository;
        private readonly IRepository<Allocation> _allocationRepository;
        private readonly IRepository<ProductionOrder> _productionOrderRepository;
        private readonly AppDbContext _context;

        public ProcessingTicketService(
            IRepository<ProcessingTicket> processingTicketRepository,
            IRepository<ProcessingTicketDetail> processingTicketDetailRepository,
            IRepository<Allocation> allocationRepository,
            IRepository<ProductionOrder> productionOrderRepository,
            AppDbContext context)
        {
            _processingTicketRepository = processingTicketRepository;
            _processingTicketDetailRepository = processingTicketDetailRepository;
            _allocationRepository = allocationRepository;
            _productionOrderRepository = productionOrderRepository;
            _context = context;
        }

        public async Task<IEnumerable<ProcessingTicket>> GetAllProcessingTicketsAsync()
        {
            return await _context.ProcessingTickets
                .Include(pt => pt.Details)
                .Include(pt => pt.ProductionOrder)
                .Include(pt => pt.ProcessingStage)
                .Where(pt => !pt.IsDeleted)
                .ToListAsync();
        }

        public async Task<ProcessingTicket?> GetProcessingTicketByIdAsync(int id)
        {
            return await _context.ProcessingTickets
                .Include(pt => pt.Details)
                .Include(pt => pt.ProductionOrder)
                .Include(pt => pt.ProcessingStage)
                .FirstOrDefaultAsync(pt => pt.Id == id && !pt.IsDeleted);
        }

        public async Task CreateProcessingTicketAsync(ProcessingTicket processingTicket)
        {
            processingTicket.ProcessingTicketNumber = await GenerateProcessingTicketNumberAsync();
            await _processingTicketRepository.AddAsync(processingTicket);
        }

        public async Task UpdateProcessingTicketAsync(ProcessingTicket processingTicket)
        {
            await _processingTicketRepository.UpdateAsync(processingTicket);
        }

        public async Task DeleteProcessingTicketAsync(int id)
        {
            await _processingTicketRepository.DeleteAsync(id);
        }

        public async Task<string> GenerateProcessingTicketNumberAsync()
        {
            var year = DateTime.Now.ToString("yy");
            var lastTicket = await _context.ProcessingTickets
                .OrderByDescending(pt => pt.ProcessingTicketNumber)
                .FirstOrDefaultAsync();

            int nextNumber = lastTicket == null ? 1 : int.Parse(lastTicket.ProcessingTicketNumber.Split('-')[2]) + 1;
            return $"PC-{year}-{nextNumber:D3}";
        }

        public async Task<List<SelectListItem>> GetActiveProductionOrdersAsync()
        {
            var productionOrders = await _context.ProductionOrders
                .Where(po => po.Status == "Đang sản xuất" && !po.IsDeleted)
                .Select(po => new SelectListItem
                {
                    Value = po.Id.ToString(),
                    Text = $"{po.ProductionOrderNumber} ({po.CustomerName})"
                }).ToListAsync();
            return productionOrders ?? new List<SelectListItem>();
        }

        public async Task<List<SelectListItem>> GetAvailableAllocationsAsync(int productionOrderId)
        {
            var allocations = await _context.Allocations
                .Include(a => a.Lot)
                .Where(a => a.ProductionOrderId == productionOrderId && !a.IsDeleted)
                .Where(a => a.Lot.InputQuantity > a.Lot.AllocatedQuantity)
                .Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = $"{a.Lot.LotNumber} - {a.Lot.ProductName} ({a.Size})"
                }).ToListAsync();
            return allocations ?? new List<SelectListItem>();
        }

        public async Task CompleteProcessingTicketAsync(int id, List<ProcessingTicketDetailViewModel> outputDetails, string action, IInventoryService inventoryService)
        {
            var ticket = await GetProcessingTicketByIdAsync(id);
            if (ticket == null) throw new Exception("Không tìm thấy phiếu chế biến");

            ticket.Status = "Hoàn thành";
            foreach (var detail in ticket.Details)
            {
                var output = outputDetails.FirstOrDefault(d => d.Id == detail.Id);
                if (output != null)
                {
                    detail.OutputProductName = output.OutputProductName;
                    detail.OutputSize = output.OutputSize;
                    detail.OutputQuantity = output.OutputQuantity;
                }
            }

            await UpdateProcessingTicketAsync(ticket);

            if (action == "Nhập kho BTP" || action == "Nhập kho TP")
            {
                foreach (var detail in ticket.Details)
                {
                    if (detail.OutputQuantity > 0)
                    {
                        var inventory = new Inventory
                        {
                            LotNumber = ticket.ProcessingTicketNumber + "-" + detail.Id,
                            ProductName = detail.OutputProductName,
                            ProductType = action == "Nhập kho BTP" ? "BTP" : "TP",
                            Size = detail.OutputSize,
                            InputQuantity = detail.OutputQuantity,
                            InputDate = DateTime.Now,
                            Note = $"Nhập từ phiếu chế biến {ticket.ProcessingTicketNumber}"
                        };
                        await inventoryService.CreateInventoryAsync(inventory);
                    }
                }
            }
        }

        public async Task<decimal> CalculateLaborCostAsync(int processingTicketId)
        {
            var ticket = await GetProcessingTicketByIdAsync(processingTicketId);
            if (ticket == null) return 0;

            var wageRate = await _context.ProductWageRates
                .Where(w => w.ProcessingStageId == ticket.ProcessingStageId && !w.IsDeleted)
                .FirstOrDefaultAsync();
            if (wageRate == null) return 0;

            var totalQuantity = ticket.Details.Sum(d => d.OutputQuantity);
            return totalQuantity * wageRate.WageRate;
        }
    }
}