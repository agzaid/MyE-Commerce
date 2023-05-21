namespace Web.Areas.Admin.Models.Sales
{
    public class ProductSalesTable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public string Barcode { get; set; }
    }
}
