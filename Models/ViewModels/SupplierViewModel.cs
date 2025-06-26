using System.ComponentModel.DataAnnotations;

namespace SeafoodApp.Models.ViewModels
{
    public class SupplierViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên nhà cung cấp là bắt buộc")]
        [StringLength(100, ErrorMessage = "Tên không được vượt quá 100 ký tự")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Địa chỉ là bắt buộc")]
        [StringLength(200, ErrorMessage = "Địa chỉ không được vượt quá 200 ký tự")]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mã số thuế là bắt buộc")]
        [StringLength(20, ErrorMessage = "Mã số thuế không được vượt quá 20 ký tự")]
        public string TaxCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Người liên hệ là bắt buộc")]
        [StringLength(50, ErrorMessage = "Tên người liên hệ không được vượt quá 50 ký tự")]
        public string ContactPerson { get; set; } = string.Empty;

        [Required(ErrorMessage = "Số điện thoại là bắt buộc")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [StringLength(15, ErrorMessage = "Số điện thoại không được vượt quá 15 ký tự")]
        public string Phone { get; set; } = string.Empty;
    }
}