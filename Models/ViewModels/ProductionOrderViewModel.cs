using System.ComponentModel.DataAnnotations;

namespace SeafoodApp.Models.ViewModels
{
    public class ProductionOrderViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Số lệnh sản xuất là bắt buộc")]
        public string ProductionOrderNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Số hợp đồng là bắt buộc")]
        public string ContractNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tên khách hàng là bắt buộc")]
        public string CustomerName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ngày cung cấp bao bì là bắt buộc")]
        public DateTime PackagingSupplyDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Ngày hoàn thành là bắt buộc")]
        public DateTime CompletionDate { get; set; } = DateTime.Now;

        public string Status { get; set; } = "Đang sản xuất";

        public List<ProductionOrderDetailViewModel> Details { get; set; } = new();
    }

    public class ProductionOrderDetailViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm là bắt buộc")]
        public string ProductName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Size là bắt buộc")]
        public string Size { get; set; } = string.Empty;

        [Required(ErrorMessage = "Đóng gói là bắt buộc")]
        public string Packaging { get; set; } = string.Empty;

        [Range(0, double.MaxValue, ErrorMessage = "Số lượng phải lớn hơn hoặc bằng 0")]
        public decimal Quantity { get; set; }

        public string Note { get; set; } = string.Empty;
    }
}