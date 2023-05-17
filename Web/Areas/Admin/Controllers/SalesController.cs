using Microsoft.AspNetCore.Mvc;
using Services.Cashier;
using Services.Shop.CategoryRepo;
using Services.Shop;
using System.Text.Json;
using Data.Entities.Cashier;
using Web.Areas.Admin.Models.Cashier;
using Data.Entities.Enums;
using Web.Controllers;
using Web.Areas.Admin.Models.Sales;
using Web.Areas.Admin.Models.Shop;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Data.Entities.Shop;
using Repo.Migrations;
using Data.Entities.Sales;

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

        [HttpGet]
        public IActionResult Create(List<string> message)
        {
            var product = new CreateSalesInvoiceViewModel();
            //var product = new CreateSkuMainItemViewModel();
            //var categories = _categoryService.GetMany(s => true, new List<string>());
            //product.ListOfCategories = categories.Select(s => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem()
            //{
            //    Text = s.Name,
            //    Value = s.ID.ToString(),
            //}).ToList();
            //product.Status = RecordStatus.Published;
            var columns = new List<string>()
            {
                 "Name",
                "Price",
                "Quantity",
            };
            ViewBag.columns = JsonSerializer.Serialize(columns);
            ViewBag.stringColumns = columns;

            //ViewBag.records = _skuProductService.GetMany(s => true, null).Count();
            //ViewBag.records_create = 0;

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync(CreateSalesInvoiceViewModel model)
        {
            var Message = new List<string>();
            if (ModelState.IsValid)
            {
                var invoice = new Invoice()
                {
                    
                };
                //var product = new Product()
                //{
                //    ThumbnailImage = model.ThumbnailFormFile != null ? $"/Uploads/Products/{await model.ThumbnailFormFile.CreateFile("Products")}" : "",
                //    CreatedDate = DateTime.UtcNow,
                //    DisplayOrder = model.DisplayOrder,
                //    ModifiedDate = DateTime.UtcNow,
                //    Price = model.Price,
                //    ProductName = model.ProductName,
                //    Quantity = model.Quantity,
                //    ShortDescription = model.ShortDescription,
                //    Status = model.Status,
                //};
                //_productService.Insert(product);
                Message.Add("Create");
                return View("Create", new { message = Message });
            }

            Message.Add("Error");
            return View("Create", new { message = Message });

        }


        [HttpGet]
        public async Task<IActionResult> GetItem(string id)
        {
            var skuSubProduct = await _skuSubItemService.GetOne(s => s.BarCodeNumber == id, new List<string>() { "SkuMainItem" });
            if (skuSubProduct == null)
            {
                return NotFound("Item not found...!!!");
            }

            var productTable = new ProductSalesTable()
            {
                Id = skuSubProduct.ID,
                Name = skuSubProduct.SkuMainItem.Name,
                Quantity = 1,
                Price = (double)skuSubProduct.Price
            };

            return Ok(new { data = productTable });
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
