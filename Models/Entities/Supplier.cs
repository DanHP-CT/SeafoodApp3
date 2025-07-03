namespace SeafoodApp.Models.Entities
{
    public class Supplier
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; // Thêm Name
        public string ContactPerson { get; set; } = string.Empty; // Thêm ContactPerson
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string TaxCode { get; set; } = string.Empty; // Thêm TaxCode
        public bool IsDeleted { get; set; }
    }
}