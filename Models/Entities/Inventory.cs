namespace SeafoodApp.Models.Entities
{
    public class Inventory
    {
        public int Id { get; set; }
        public string LotNumber { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string ProductType { get; set; } = "BTP"; // BTP hoặc TP
        public string Size { get; set; } = string.Empty;
        public decimal InputQuantity { get; set; }
        public decimal OutputQuantity { get; set; }
        public decimal StockQuantity { get; set; } // Tồn kho = Input - Output
        public DateTime InputDate { get; set; }
        public DateTime? OutputDate { get; set; }
        public string Note { get; set; } = string.Empty;
        public bool IsDeleted { get; set; }
    }
}