namespace SeafoodApp.Models.Entities
{
    public class Allocation
    {
        public int Id { get; set; }
        public int PurchaseOrderDetailId { get; set; }
        public PurchaseOrderDetail PurchaseOrderDetail { get; set; } = null!;
        public int ProductionOrderId { get; set; }
        public ProductionOrder ProductionOrder { get; set; } = null!;
        public int LotId { get; set; }
        public Lot Lot { get; set; } = null!;
        public string Size { get; set; } = string.Empty;
        public decimal AllocatedQuantity { get; set; } // Thêm AllocatedQuantity
        public bool IsDeleted { get; set; }
    }
}