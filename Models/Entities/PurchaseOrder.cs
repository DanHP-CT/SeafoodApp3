namespace SeafoodApp.Models.Entities
{
    public class PurchaseOrder
    {
        public int Id { get; set; }
        public string PurchaseOrderNumber { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public DateTime SupplyDate { get; set; }
        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; } = null!;
        public string Status { get; set; } = "Chưa điều phối";
        public decimal TotalQuantity { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsDeleted { get; set; }
        public List<PurchaseOrderDetail> Details { get; set; } = new();
        public List<Lot> Lots { get; set; } = new();
    }
}