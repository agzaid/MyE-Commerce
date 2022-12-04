using Microsoft.AspNetCore.Mvc;
using Services.Shop;

namespace Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        public IActionResult Index()
        {
            var products = _productService.GetMany(s => true, null);
            return View(products);
        }
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var product = await _productService.GetOne(s => s.ID == id, null);
            if (product is null)
                return NotFound();

            return View(product);
        }

    }
}
