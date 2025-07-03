namespace SeafoodApp.Models.Entities
{
    public class ProductWageRate
    {
        public int Id { get; set; }
        public int ProcessingStageId { get; set; }
        public ProcessingStage ProcessingStage { get; set; } = null!;
        public decimal WageRate { get; set; } // Đơn giá lương (VND/kg)
        public bool IsDeleted { get; set; }
    }
}