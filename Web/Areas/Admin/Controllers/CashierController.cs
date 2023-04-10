using Data.Entities.Cashier;
using Data.Entities.Enums;
using Data.Entities.Shop;
using Microsoft.AspNetCore.Mvc;
using Services.Shop;
using System.Text.Json;
using Web.Areas.Admin.Models.Shop;
using Web.Areas.Admin.Models.Cashier;

namespace Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CashierController : Controller
    {
        private readonly ISkuProductService _skuProductService;

        public CashierController(ISkuProductService skuProductService)
        {
            _skuProductService = skuProductService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ListOfSkuProducts(List<string> message)
        {
            if (message.Count > 0)
            {
                ViewBag.Message = message[0];
            }
            var skuproducts = _skuProductService.GetMany(s => true, null);

            var columns = new List<string>()
            {
                "Name",
                "BarCodeNumber",
                "Price",
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
            var product = new CreateSkuProductViewModel();
            var categories = _skuProductService.GetMany(s => true, null);
            
            return View(product);
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> CreateAsync(CreateProductViewModel model)
        //{
        //    var Message = new List<string>();
        //    if (ModelState.IsValid)
        //    {
        //        var product = new Product()
        //        {
        //            ThumbnailImage = $"/Uploads/Products/{await model.ThumbnailFormFile.CreateFile("Products")}",
        //            CreatedDate = DateTime.UtcNow,
        //            DisplayOrder = model.DisplayOrder,
        //            ModifiedDate = DateTime.UtcNow,
        //            Price = model.Price,
        //            ProductName = model.ProductName,
        //            Quantity = model.Quantity,
        //            ShortDescription = model.ShortDescription,
        //            Status = model.Status,
        //        };
        //        _productService.Insert(product);
        //        Message.Add("Create");
        //    }
        //    return RedirectToAction("index", new { message = Message });
        //}

       


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
            IEnumerable<SkuProduct> skuproducts = _skuProductService.GetMany(s => true, null)
                .Where(m => string.IsNullOrEmpty(searchValue) ? true : (m.Name.Contains(searchValue) || m.ShortDescription.Contains(searchValue) || m.Price.ToString().Contains(searchValue) || m.BarCodeNumber.ToString().Contains(searchValue)));

            var model = skuproducts.Select(s => new ListOfSkuProductsViewModel()
            {
                ID = s.ID,
                Name = s.Name,
                Price = s.Price,
                BarCodeNumber = s.BarCodeNumber,
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

        #endregion
    }
}
