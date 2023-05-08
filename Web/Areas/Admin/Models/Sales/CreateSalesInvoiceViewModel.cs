﻿namespace Web.Areas.Admin.Models.Sales
{
	public class CreateSalesInvoiceViewModel
	{
		public int InvoiceId { get; set; } = new Random().Next();
		public string ProductName { get; set; }
		public int AvailableQuantity { get; set; }
		public int Quantity { get; set; }
		public double UnitPrice { get; set; }
		public string Description { get; set; }
		public double? TotalPrice { get; set; }
		public double? Tendered { get; set; }
		public double? Change { get; set; }
	}
}
