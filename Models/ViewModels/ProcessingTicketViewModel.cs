using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SeafoodApp.Models.ViewModels
{
    public class ProcessingTicketViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Số phiếu chế biến là bắt buộc")]
        public string ProcessingTicketNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Lệnh sản xuất là bắt buộc")]
        public int ProductionOrderId { get; set; }
        public string ProductionOrderNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Công đoạn chế biến là bắt buộc")]
        public int ProcessingStageId { get; set; } // Thêm trường công đoạn
        public string ProcessingStageName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ngày chế biến là bắt buộc")]
        public DateTime ProcessingDate { get; set; } = DateTime.Now;

        public string Status { get; set; } = "Đang chế biến";

        public List<ProcessingTicketDetailViewModel> Details { get; set; } = new();

        public List<SelectListItem> ProductionOrders { get; set; } = new();
        public List<SelectListItem> ProcessingStages { get; set; } = new(); // Thêm dropdown công đoạn
    }

    public class ProcessingTicketDetailViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Số lô là bắt buộc")]
        public int AllocationId { get; set; }
        public string LotNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tên sản phẩm là bắt buộc")]
        public string ProductName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Size là bắt buộc")]
        public string Size { get; set; } = string.Empty;

        [Range(0, double.MaxValue, ErrorMessage = "Số lượng chế biến phải lớn hơn hoặc bằng 0")]
        public decimal ProcessedQuantity { get; set; }

        public string OutputProductName { get; set; } = string.Empty;
        public string OutputSize { get; set; } = string.Empty;

        [Range(0, double.MaxValue, ErrorMessage = "Số lượng đầu ra phải lớn hơn hoặc bằng 0")]
        public decimal OutputQuantity { get; set; }
    }
}