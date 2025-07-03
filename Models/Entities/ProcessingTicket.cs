namespace SeafoodApp.Models.Entities
{
    public class ProcessingTicket
    {
        public int Id { get; set; }
        public string ProcessingTicketNumber { get; set; } = string.Empty;
        public int ProductionOrderId { get; set; }
        public ProductionOrder ProductionOrder { get; set; } = null!;
        public int ProcessingStageId { get; set; } // Thêm trường công đoạn
        public ProcessingStage ProcessingStage { get; set; } = null!;
        public DateTime ProcessingDate { get; set; }
        public string Status { get; set; } = "Đang chế biến";
        public bool IsDeleted { get; set; }
        public List<ProcessingTicketDetail> Details { get; set; } = new();
    }
}