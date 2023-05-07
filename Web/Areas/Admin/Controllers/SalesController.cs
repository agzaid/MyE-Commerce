using Microsoft.AspNetCore.Mvc;
using Services.Cashier;
using Services.Shop.CategoryRepo;
using Services.Shop;
using System.Text.Json;
using Data.Entities.Cashier;
using Web.Areas.Admin.Models.Cashier;
using Data.Entities.Enums;
using Web.Controllers;

namespace Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SalesController : BaseController<SalesController>
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
            var product = new CreateSkuMainItemViewModel();
            var categories = _categoryService.GetMany(s => true, new List<string>());
            product.ListOfCategories = categories.Select(s => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem()
            {
                Text = s.Name,
                Value = s.ID.ToString(),
            }).ToList();
            product.Status = RecordStatus.Published;
            var columns = new List<string>()
            {
                // "Name",
                //"Price",
                //"Quantity",
                //"ShortDescription",
                //"Status"
                "BarcodeNumber",
                "Price",
                "ExpiryDate",
                "Status"
            };
            ViewBag.columns = JsonSerializer.Serialize(columns);
            ViewBag.stringColumns = columns;

            //ViewBag.records = _skuProductService.GetMany(s => true, null).Count();
            ViewBag.records_create = 0;

            return View(product);
        }

		[HttpGet]
		public async Task<IActionResult> GetItem(string id)
		{
			var skuProduct = await _skuProductService.GetOne(s => true, new List<string>() { "skuSubItems" });
			if (skuProduct == null)
			{
				return NotFound();
			}
			var categories = _categoryService.GetMany(s => true, null);

			var productViewModel = new EditSkuMainItemViewModel()
			{
				Id = skuProduct.ID,
				Name = skuProduct.Name,
				PurchasePrice = skuProduct.PurchasePrice,
				Quantity = skuProduct.Quantity,
				ShortDescription = skuProduct.ShortDescription,
				CategoryId = skuProduct.CategoryId,
				Status = skuProduct.Status,
				ListSkuSubItems = skuProduct.skuSubItems.Select(s => new SkuSubItemViewModel()
				{
					BarCodeNumber = s.BarCodeNumber,
					//ExpiryDate = s.ExpiryDate?.ToString("yyyyMMddHHmmss"),
					ExpiryDate = s.ExpiryDate?.ToString("yyyy-MM-dd"),
					Status = s.Status,
					Price = s.Price,
					ID = s.ID

				}).ToList(),
				ThumbnailImage = skuProduct.ThumbnailImage
			};
			productViewModel.ListOfCategories = categories.Select(s => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem()
			{
				Text = s.Name,
				Value = s.ID.ToString(),
			}).ToList();
			var columns = new List<string>()
			{
				"BarCodeNumber",
				"Price",
				"ExpiryDate",
				"Status"
			};
			ViewBag.columns = JsonSerializer.Serialize(columns);
			ViewBag.stringColumns = columns;


			return View(productViewModel);
		}




		[HttpPost]
        public IActionResult LoadDataTable()
        {
            var pageSize = int.Parse(Request.Form["length"]);
            var skip = int.Parse(Request.Form["start"]);
            var searchValue = Request.Form["search[value]"];
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"] + "][name]"];
            var sortDir = Request.Form["order[0][dir]"];

            //for searching
            IEnumerable<SkuMainItem> skuproducts = _skuProductService.GetMany(s => true, null)
                .Where(m => m.Name.ToLower().Contains(searchValue.ToString())
                || (m.Status.ToString().Contains(searchValue)));

            var model = skuproducts.Select(s => new ListOfSkuMainItemViewModel()
            {
                ID = s.ID,
                Name = s.Name,
                PurchasePrice = s.PurchasePrice,
                Quantity = s.Quantity,
                Status = s.Status,
                ThumbnailImage = s.ThumbnailImage,
                ShortDescription = s.ShortDescription,
            });
            var data = model.Skip(skip).Take(pageSize).ToList();

            var recordsTotal = model.Count();
            ViewData["records"] = recordsTotal;
            return Ok(new { recordsFiltered = recordsTotal, recordsTotal, data = data });
        }


    }
}
