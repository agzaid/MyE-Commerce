namespace Web.Models.Shop
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public List<string> Images { get; set; }
        public List<IFormFile> ImageFiles { get; set; }
        public string Description { get; set; }
    }
}
