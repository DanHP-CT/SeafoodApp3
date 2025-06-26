namespace SeafoodApp.Models.Entities
{
    public class ProductionOrder
    {
        public int Id { get; set; }
        public string ProductionOrderNumber { get; set; } = string.Empty;
        public string ContractNumber { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public DateTime PackagingSupplyDate { get; set; }
        public DateTime CompletionDate { get; set; }
        public string Status { get; set; } = "Đang sản xuất";
        public bool IsDeleted { get; set; }
        public List<ProductionOrderDetail> Details { get; set; } = new();
        public List<Allocation> Allocations { get; set; } = new();
    }
}