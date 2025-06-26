namespace SeafoodApp.Models.Entities
{
    public class Lot
    {
        public int Id { get; set; }
        public string LotNumber { get; set; } = string.Empty;
        public int PurchaseOrderId { get; set; }
        public PurchaseOrder PurchaseOrder { get; set; } = null!;
        public string ProductName { get; set; } = string.Empty;
        public decimal InputQuantity { get; set; }
        public decimal AllocatedQuantity { get; set; }
        public bool IsDeleted { get; set; }
    }
}