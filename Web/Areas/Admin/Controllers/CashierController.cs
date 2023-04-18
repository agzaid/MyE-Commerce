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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync(CreateSkuMainItemViewModel model)
        {
            var Message = new List<string>();
            if (ModelState.IsValid)
            {
                var skuMainItem = new SkuMainItem()
                {
                    ID = 0,
                    Name = model.Name,
                    ShortDescription = model.ShortDescription,
                    Status = SkuItemStatus.available,
                    CategoryId = model.CategoryId,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    Quantity = 0,
                    ThumbnailImage = model.ThumbnailFormFile != null ? ($"/Uploads/SkuMainItems/{await model.ThumbnailFormFile.CreateFile("SkuMainItems")}") : "",
                    skuSubItems = new List<SkuSubItem>()
                };
                if (model.ListSkuSubItems.Count != 0)
                {
                    model.ListSkuSubItems.ForEach(s =>
                    {
                        skuMainItem.skuSubItems.Add(new SkuSubItem()
                        {
                            Price = s.Price,
                            BarCodeNumber = s.BarCodeNumber,
                            CreatedDate = DateTime.Now,
                            ExpiryDate = s.ExpiryDate,
                            ModifiedDate = DateTime.Now,
                            SkuMainItem = skuMainItem,
                            SkuMainItemId = skuMainItem.ID,
                            Status = s.Status
                        });

                    });
                    skuMainItem.Quantity = model.ListSkuSubItems.Count();
                    skuMainItem.Price = model.ListSkuSubItems.OrderByDescending(s => s.Price).First().Price;
                };

                _skuProductService.Insert(skuMainItem);
                Message.Add("Create");
            }
            return RedirectToAction("index", new { message = Message });
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var skuProduct = await _skuProductService.GetOne(s => s.ID == id, new List<string>() { "skuSubItems" });
            if (skuProduct == null)
            {
                return NotFound();
            }
            var categories = _categoryService.GetMany(s => true, null);

            var productViewModel = new EditSkuMainItemViewModel()
            {
                Id = skuProduct.ID,
                Name = skuProduct.Name,
                Price = skuProduct.Price,
                Quantity = skuProduct.Quantity,
                ShortDescription = skuProduct.ShortDescription,
                CategoryId = skuProduct.CategoryId,
                Status = skuProduct.Status,
                ListSkuSubItems = skuProduct.skuSubItems.Select(s => new SkuSubItemViewModel()
                {
                    BarCodeNumber = s.BarCodeNumber,
                    ExpiryDate = s.ExpiryDate,
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
                "BarcodeNumber",
                "Price",
                "ExpiryDate",
                "Status"
            };
            ViewBag.columns = JsonSerializer.Serialize(columns);
            ViewBag.stringColumns = columns;


            return View(productViewModel);
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
                .Where(m => string.IsNullOrEmpty(searchValue) ? true : (m.Name.Contains(searchValue) || m.ShortDescription.Contains(searchValue) || m.Price.ToString().Contains(searchValue)));
            var model = skuproducts.Select(s => new ListOfSkuMainItemViewModel()
            {
                ID = s.ID,
                Name = s.Name,
                Price = s.Price,
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

        [HttpPost]
        public IActionResult Create_LoadDataTable()
        {
            var pageSize = int.Parse(Request.Form["length"]);
            var skip = int.Parse(Request.Form["start"]);
            var searchValue = Request.Form["search[value]"];
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"] + "][name]"];
            var sortDir = Request.Form["order[0][dir]"];

            //for searching
            IEnumerable<SkuMainItem> skuproducts = new List<SkuMainItem>();
            //not showing cause of previous line
            var model = skuproducts.Select(s => new ListOfSkuMainItemViewModel()
            {
                ID = s.ID,
                Name = s.Name,
                Quantity = s.Quantity,
                Price = s.Price,
                Status = s.Status,
                ThumbnailImage = s.ThumbnailImage,
                ShortDescription = s.ShortDescription,
            });
            //var model = skuproducts.Select(s => s.skuSubItems.Select(a => new SkuSubItemViewModel()
            //{
            //    ID = a.ID,
            //    BarCodeNumber = a.BarCodeNumber,
            //    Price = a.Price,
            //    ExpiryDate = a.ExpiryDate,
            //    Status = a.Status
            //}));
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
        public IActionResult Edit_LoadDataTable(int id)
        {
            var pageSize = int.Parse(Request.Form["length"]);
            var skip = int.Parse(Request.Form["start"]);
            var searchValue = Request.Form["search[value]"];
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"] + "][name]"];
            var sortDir = Request.Form["order[0][dir]"];

            //for searching
            //IEnumerable<SkuMainItem> skuproducts = new List<SkuMainItem>();
            IEnumerable<SkuMainItem> skuproducts = _skuProductService.GetMany(s => s.ID == id, new List<string>() { "skuSubItems" })
               .Where(m => string.IsNullOrEmpty(searchValue) ? true : (m.Name.Contains(searchValue) || m.ShortDescription.Contains(searchValue) || m.Price.ToString().Contains(searchValue)));

            var model = skuproducts.Select(s => s.skuSubItems.Select(e => new SkuSubItemViewModel()
            {
                ID = e.ID,
                BarCodeNumber = e.BarCodeNumber,
                ExpiryDate = e.ExpiryDate,
                Price = e.Price,
                Status = e.Status,
            }));
            //var model = skuproducts.Select(s => new ListOfSkuMainItemViewModel()
            //{
            //    ID = s.ID,
            //    Name = s.Name,
            //    Quantity = s.Quantity,
            //    Price = (double)s.Price,
            //    Status = s.Status,
            //    ThumbnailImage = s.ThumbnailImage,
            //    ShortDescription = s.ShortDescription,
            //});

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
