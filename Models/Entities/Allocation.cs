namespace SeafoodApp.Models.Entities
{
    public class Allocation
    {
        public int Id { get; set; }
        public int LotId { get; set; }
        public Lot Lot { get; set; } = null!;
        public string Size { get; set; } = string.Empty;
        public decimal AllocatedQuantity { get; set; }
        public int ProductionOrderId { get; set; } // LSX
        public bool IsDeleted { get; set; }
    }
}