using Data.Entities.Cashier;
using Data.Entities.Enums;
using Data.Entities.Shop;
using Microsoft.AspNetCore.Mvc;
using Services.Shop;
using System.Text.Json;
using Web.Areas.Admin.Models.Shop;
using Web.Areas.Admin.Models.Cashier;
using Services.Injection;
using Services.Shop.CategoryRepo;

namespace Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CashierController : Controller
    {
        private readonly ISkuMainItemService _skuProductService;
        private readonly ICategoryService _categoryService;

        public CashierController(ISkuMainItemService skuProductService, ICategoryService categoryService)
        {
            _skuProductService = skuProductService;
            _categoryService = categoryService;
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

        [HttpGet]
        public IActionResult Create()
        {
            var product = new CreateSkuMainItemViewModel();
            var categories = _categoryService.GetMany(s => true, new List<string>());
            product.ListOfCategories = categories.Select(s => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem()
            {
                Text = s.Name,
                Value = s.ID.ToString(),
            }).ToList();
            product.Status = SkuItemStatus.available;
            var columns = new List<string>()
            {
                //"Name",
                "BarCodeNumber",
                "Price",
                "ExpiryDate",
                "Status"
            };
            ViewBag.columns = JsonSerializer.Serialize(columns);
            
            ViewBag.records = _skuProductService.GetMany(s => true, null).Count();

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync(CreateSkuMainItemViewModel model)
        {
            var Message = new List<string>();
            if (ModelState.IsValid)
            {
                var product = new Product()
                {
                    ThumbnailImage = $"/Uploads/SkuMainItems/{await model.ThumbnailFormFile.CreateFile("SkuMainItems")}",
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow,
                    Price = model.Price,
                    Quantity = model.Quantity,
                    ShortDescription = model.ShortDescription,
                };
                //_productService.Insert(product);
                Message.Add("Create");
            }
            return RedirectToAction("index", new { message = Message });
        }




        #region Helper Methods

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
                .Where(m => string.IsNullOrEmpty(searchValue) ? true : (m.Name.Contains(searchValue) || m.ShortDescription.Contains(searchValue)  || m.Price.ToString().Contains(searchValue)));

            var model = skuproducts.Select(s => new ListOfSkuMainItemViewModel()
            {
                ID = s.ID,
                Name = s.Name,
                Price = (double)s.Price,
                Quantity = s.Quantity,
                Status = s.Status,
                ThumbnailImage = s.ThumbnailImage,
                ShortDescription = s.ShortDescription,
            });
            //for sorting
            //IQueryable<Product> queryProducts = (IQueryable<Product>)products;
            //if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortDir)))
            //   products = queryProducts.OrderBy(string.Concat(sortColumn, " ", sortDir));

            var data = model.Skip(skip).Take(pageSize).ToList();

            var recordsTotal = model.Count();
            ViewData["records"] = recordsTotal;
            return Ok(new { recordsFiltered = recordsTotal, recordsTotal, data = data });
        }
        
        [HttpPost]
        public IActionResult Sub_LoadDataTable()
        {
            var pageSize = int.Parse(Request.Form["length"]);
            var skip = int.Parse(Request.Form["start"]);
            var searchValue = Request.Form["search[value]"];
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"] + "][name]"];
            var sortDir = Request.Form["order[0][dir]"];

            //for searching
            IEnumerable<SkuMainItem> skuproducts = _skuProductService.GetMany(s => true, null)
                .Where(m => string.IsNullOrEmpty(searchValue) ? true : (m.Name.Contains(searchValue) || m.ShortDescription.Contains(searchValue)  || m.Price.ToString().Contains(searchValue)));

            var model = skuproducts.Select(s => new ListOfSkuMainItemViewModel()
            {
                ID = s.ID,
                //Name = s.Name,
                Quantity = s.Quantity,
                Price = s.Price,
                Status = s.Status,
                ThumbnailImage = s.ThumbnailImage,
            });
            //for sorting
            //IQueryable<Product> queryProducts = (IQueryable<Product>)products;
            //if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortDir)))
            //   products = queryProducts.OrderBy(string.Concat(sortColumn, " ", sortDir));

            var data = model.Skip(skip).Take(pageSize).ToList();

            var recordsTotal = model.Count();
            ViewData["records"] = recordsTotal;
            return Ok(new { recordsFiltered = recordsTotal, recordsTotal, data = data });
        }

        #endregion
    }
}
