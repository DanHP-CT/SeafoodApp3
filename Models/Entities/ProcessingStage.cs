namespace SeafoodApp.Models.Entities
{
    public class ProcessingStage
    {
        public int Id { get; set; }
        public string StageName { get; set; } = string.Empty; // Tên công đoạn
        public string Description { get; set; } = string.Empty; // Mô tả quy trình
        public string InputMaterial { get; set; } = string.Empty; // Nguyên liệu đầu vào
        public string OutputProduct { get; set; } = string.Empty; // Sản phẩm đầu ra
        public decimal StandardRate { get; set; } // Định mức (kg/người/giờ)
        public bool IsDeleted { get; set; }
    }
}