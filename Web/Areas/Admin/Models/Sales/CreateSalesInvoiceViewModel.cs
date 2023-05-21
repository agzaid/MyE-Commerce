using Web.Areas.Admin.Models.Cashier;

namespace Web.Areas.Admin.Models.Sales
{
	public class CreateSalesInvoiceViewModel
	{
		public int InvoiceNo { get; set; } = new Random().Next();
		public string ProductName { get; set; }
		public string DateCreated { get; set; }
		public int AvailableQuantity { get; set; }
		public int TotalQuantity { get; set; }
		public double UnitPrice { get; set; }
		public string Description { get; set; }
		public double? TotalPrice { get; set; }
		public double? Tendered { get; set; }
		public double? Change { get; set; }
		public List<InvoiceItemsViewModel> InvoiceItems { get; set; } = new ();

    }
  
}
