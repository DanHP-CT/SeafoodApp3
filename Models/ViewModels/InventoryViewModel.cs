using Microsoft.AspNetCore.Mvc.Rendering;

namespace SeafoodApp.Models.ViewModels
{
    public class InventoryViewModel
    {
        public int Id { get; set; }
        public string LotNumber { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string ProductType { get; set; } = "BTP";
        public string Size { get; set; } = string.Empty;
        public decimal InputQuantity { get; set; }
        public decimal OutputQuantity { get; set; }
        public decimal StockQuantity { get; set; }
        public DateTime InputDate { get; set; }
        public DateTime? OutputDate { get; set; }
        public string Note { get; set; } = string.Empty;
    }

    public class InventoryFilterViewModel
    {
        public string LotNumber { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string ProductType { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;
        public List<SelectListItem> ProductTypes { get; set; } = new()
        {
            new SelectListItem { Value = "", Text = "Tất cả" },
            new SelectListItem { Value = "BTP", Text = "Bán thành phẩm" },
            new SelectListItem { Value = "TP", Text = "Thành phẩm" }
        };
        public List<InventoryViewModel> Inventories { get; set; } = new();
    }
}