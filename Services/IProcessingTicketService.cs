using SeafoodApp.Models.Entities;
using SeafoodApp.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SeafoodApp.Services
{
    public interface IProcessingTicketService
    {
        Task<IEnumerable<ProcessingTicket>> GetAllProcessingTicketsAsync();
        Task<ProcessingTicket?> GetProcessingTicketByIdAsync(int id);
        Task CreateProcessingTicketAsync(ProcessingTicket processingTicket);
        Task UpdateProcessingTicketAsync(ProcessingTicket processingTicket);
        Task DeleteProcessingTicketAsync(int id);
        Task<string> GenerateProcessingTicketNumberAsync();
        Task<List<SelectListItem>> GetActiveProductionOrdersAsync();
        Task<List<SelectListItem>> GetAvailableAllocationsAsync(int productionOrderId);
        Task CompleteProcessingTicketAsync(int id, List<ProcessingTicketDetailViewModel> outputDetails, string action, IInventoryService inventoryService);
        Task<decimal> CalculateLaborCostAsync(int processingTicketId);
    }
}