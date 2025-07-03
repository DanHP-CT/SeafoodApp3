namespace SeafoodApp.Models.ViewModels
{
    public class ProductionOrderDetailViewModel
    {
        public int ProductionOrderId { get; set; }
        public string ProductionOrderNumber { get; set; } = string.Empty;

        // Bảng 1: Nguyên liệu từ Phiếu mua nguyên liệu
        public List<PurchaseMaterialViewModel> PurchaseMaterials { get; set; } = new();

        // Bảng 2: Nguyên liệu từ Phiếu chế biến trước
        public List<ProcessedMaterialViewModel> ProcessedMaterials { get; set; } = new();

        // Bảng 3: Bán thành phẩm trong kho
        public List<InventoryItemViewModel> InventoryItems { get; set; } = new();
    }

    public class PurchaseMaterialViewModel
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public string LotNumber { get; set; } = string.Empty;
        public decimal UsedQuantity { get; set; }
        public bool IsUsed { get; set; }
    }

    public class ProcessedMaterialViewModel
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public string LotNumber { get; set; } = string.Empty;
        public decimal UsedQuantity { get; set; }
        public bool IsUsed { get; set; }
    }

    public class InventoryItemViewModel
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public string LotNumber { get; set; } = string.Empty;
        public decimal UsedQuantity { get; set; }
        public bool IsUsed { get; set; }
    }
}