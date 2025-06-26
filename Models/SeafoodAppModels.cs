using System;
using System.Collections.Generic;

namespace SeafoodApp.Models
{
    public class Supplier
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Address { get; set; }
        public required string TaxCode { get; set; }
        public required string ContactPerson { get; set; }
        public required string PhoneNumber { get; set; }
        public List<PurchaseOrder> PurchaseOrders { get; set; } = new List<PurchaseOrder>();
    }

    public class PurchaseOrder
    {
        public int Id { get; set; }
        public required string OrderNumber { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime SupplyDate { get; set; }
        public int SupplierId { get; set; }
        public required Supplier Supplier { get; set; }
        public required string LotNumber { get; set; }
        public required string PriceListNumber { get; set; }
        public required string ProductName { get; set; }
        public decimal TotalQuantity { get; set; }
        public decimal TotalAmount { get; set; }
        public required string Status { get; set; }
        public List<PurchaseOrderDetail> Details { get; set; } = new List<PurchaseOrderDetail>();
    }

    public class PurchaseOrderDetail
    {
        public int Id { get; set; }
        public int PurchaseOrderId { get; set; }
        public required PurchaseOrder PurchaseOrder { get; set; }
        public required string Size { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
        public decimal Amount { get; set; }
        public List<Classification> Classifications { get; set; } = new List<Classification>();
    }

    public class Classification
    {
        public int Id { get; set; }
        public required string Type { get; set; }
        public required string Size { get; set; }
        public int? ProductionOrderId { get; set; } // Nullable if not yet assigned
        public ProductionOrder? ProductionOrder { get; set; } // Nullable if not yet assigned
    }

    public class ProductionOrder
    {
        public int Id { get; set; }
        public required string OrderNumber { get; set; }
        public required string ContractNumber { get; set; }
        public required string CustomerName { get; set; }
        public required string Status { get; set; }
        public required string ProductName { get; set; }
        public required string Size { get; set; }
        public required string Packaging { get; set; }
        public string? Note { get; set; } // Optional field, can be null
        public List<ProductionOrderDetail> Details { get; set; } = new List<ProductionOrderDetail>();
        public List<ProcessingSheet> ProcessingSheets { get; set; } = new List<ProcessingSheet>();
    }

    public class ProductionOrderDetail
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string Input { get; set; }
        public required string Output { get; set; }
        public required string Standard { get; set; }
    }

    public class ProcessingSheet
    {
        public int Id { get; set; }
        public int ProductionOrderId { get; set; }
        public required ProductionOrder ProductionOrder { get; set; }
        public required string SheetNumber { get; set; }
        public required string ProcessingStep { get; set; }
        public required string Department { get; set; }
        public required string LotNumber { get; set; }
        public required string ProductName { get; set; }
        public required string ProductType { get; set; }
        public required string Size { get; set; }
        public string? Note { get; set; } // Optional field, can be null
        public required string WageRate { get; set; }
    }
}
