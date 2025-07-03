using SeafoodApp.Models.ViewModels;
using System.ComponentModel.DataAnnotations;

[Required(ErrorMessage = "Production Order Number is required")]
[Display(Name = "Production Order Number")]
public string? ProductionOrderNumber { get; set; }

[Display(Name = "Contract Number")]
public string? ContractNumber { get; set; }

[Display(Name = "Customer Name")]
public string? CustomerName { get; set; }

[Display(Name = "Packaging Supply Date")]
[DataType(DataType.Date)]
public DateTime? PackagingSupplyDate { get; set; }

[Display(Name = "Completion Date")]
[DataType(DataType.Date)]
public DateTime? CompletionDate { get; set; }

[Display(Name = "Status")]
public string? Status { get; set; }

public List<ProductionOrderDetailViewModel> Details { get; set; } = new List<ProductionOrderDetailViewModel>();
}