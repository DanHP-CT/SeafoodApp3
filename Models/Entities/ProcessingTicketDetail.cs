namespace SeafoodApp.Models.Entities
{
    public class ProcessingTicketDetail
    {
        public int Id { get; set; }
        public int ProcessingTicketId { get; set; }
        public ProcessingTicket ProcessingTicket { get; set; } = null!;
        public int AllocationId { get; set; }
        public Allocation Allocation { get; set; } = null!;
        public string ProductName { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;
        public decimal ProcessedQuantity { get; set; }
        public string OutputProductName { get; set; } = string.Empty;
        public string OutputSize { get; set; } = string.Empty;
        public decimal OutputQuantity { get; set; }
        public bool IsDeleted { get; set; }
    }
}