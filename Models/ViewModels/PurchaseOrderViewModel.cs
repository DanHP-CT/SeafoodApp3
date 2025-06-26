using System.ComponentModel.DataAnnotations;

namespace SeafoodApp.Models.ViewModels
{
    public class PurchaseOrderViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Số phiếu là bắt buộc")]
        public string PurchaseOrderNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ngày lập là bắt buộc")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Ngày cung cấp là bắt buộc")]
        public DateTime SupplyDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Nhà cung cấp là bắt buộc")]
        public int SupplierId { get; set; }

        public string SupplierName { get; set; } = string.Empty;

        public string Status { get; set; } = "Chưa điều phối";

        public decimal TotalQuantity { get; set; }
        public decimal TotalAmount { get; set; }

        public List<PurchaseOrderDetailViewModel> Details { get; set; } = new();
    }

    public class PurchaseOrderDetailViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm là bắt buộc")]
        public string ProductName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Size là bắt buộc")]
        public string Size { get; set; } = string.Empty;

        [Range(0, double.MaxValue, ErrorMessage = "Giá phải lớn hơn hoặc bằng 0")]
        public decimal Price { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Số lượng phải lớn hơn hoặc bằng 0")]
        public decimal Quantity { get; set; }

        public decimal Amount { get; set; }
    }
}