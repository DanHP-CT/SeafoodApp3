namespace SeafoodApp.Models.Entities
{
    public class ProductionOrderDetail
    {
        public int Id { get; set; }
        public int ProductionOrderId { get; set; }
        public ProductionOrder ProductionOrder { get; set; } = null!;
        public string ProductName { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;
        public string Packaging { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public string Note { get; set; } = string.Empty;
        public bool IsDeleted { get; set; }
    }
}