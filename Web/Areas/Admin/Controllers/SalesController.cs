using Microsoft.AspNetCore.Mvc;
using Services.Cashier;
using Services.Shop.CategoryRepo;
using Services.Shop;
using System.Text.Json;

namespace Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SalesController : Controller
    {
        private readonly ISkuMainItemService _skuProductService;
        private readonly ICategoryService _categoryService;
        private readonly ISkuSubItemService _skuSubItemService;

        public SalesController(ISkuMainItemService skuProductService, ICategoryService categoryService, ISkuSubItemService skuSubItemService)
        {
            _skuProductService = skuProductService;
            _categoryService = categoryService;
            _skuSubItemService = skuSubItemService;
        }

        public IActionResult Index(List<string> message)
        {
            if (message.Count > 0)
            {
                ViewBag.Message = message[0];
            }
            var skuproducts = _skuProductService.GetMany(s => true, null);

            var columns = new List<string>()
            {
                "Name",
                "Price",
                "Quantity",
                "ShortDescription",
                "Status"
            };
            ViewBag.columns = JsonSerializer.Serialize(columns);
            ViewBag.stringColumns = columns;

            return View(skuproducts);
        }

    }
}
