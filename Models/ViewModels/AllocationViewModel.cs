using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SeafoodApp.Models.ViewModels
{
    public class AllocationViewModel
    {
        public int Id { get; set; }
        public int LotId { get; set; }
        public string LotNumber { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;

        [Range(0, double.MaxValue, ErrorMessage = "Số lượng điều phối phải lớn hơn hoặc bằng 0")]
        public decimal AllocatedQuantity { get; set; }

        public decimal RemainingQuantity { get; set; } // Thêm thuộc tính này

        [Required(ErrorMessage = "Lệnh sản xuất (LSX) là bắt buộc")]
        public int ProductionOrderId { get; set; }
        public List<SelectListItem> ProductionOrders { get; set; } = new();
    }

    public class AllocationListViewModel
    {
        public int PurchaseOrderId { get; set; }
        public string PurchaseOrderNumber { get; set; } = string.Empty;
        public List<AllocationViewModel> Allocations { get; set; } = new();
        public decimal TotalInputQuantity { get; set; }
        public decimal TotalAllocatedQuantity { get; set; }
        public decimal RemainingQuantity { get; set; }
    }
}