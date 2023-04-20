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
using Services.Cashier;
using System.Text.RegularExpressions;

namespace Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CashierController : Controller
    {
        private readonly ISkuMainItemService _skuProductService;
        private readonly ICategoryService _categoryService;
        private readonly ISkuSubItemService _skuSubItemService;

        public CashierController(ISkuMainItemService skuProductService, ICategoryService categoryService, ISkuSubItemService skuSubItemService)
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
                    Status = model.Status,
                    CategoryId = model.CategoryId,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    Quantity = 0,
                    ThumbnailImage = model.ThumbnailFormFile != null ? ($"/Uploads/SkuMainItems/{await model.ThumbnailFormFile.CreateFile("SkuMainItems")}") : "",
                    skuSubItems = new List<SkuSubItem>()
                };
                if (model.ListSkuSubItems.Count != 0)
                {
                    model.ListSkuSubItems.RemoveAll(s => (s.BarCodeNumber == null) || (s.Price == 0));
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
                            Status = SkuItemStatus.available
                        });

                    });
                    skuMainItem.Quantity = model.ListSkuSubItems.Count();
                    skuMainItem.Price = model.ListSkuSubItems.OrderByDescending(s => s.Price).First().Price;
                };

                _skuProductService.Insert(skuMainItem);
                Message.Add("Create");
            }
            //var errors = ModelState.Select(x => x.Value.Errors)
            //               .Where(y => y.Count > 0)
            //               .ToList();
            //errors.ForEach(s => Message.Add(s.First().ErrorMessage));
            Message.Add("ErrorSubItem");
            return RedirectToAction("index", new { message = Message });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id, List<string> message)
        {
            if (message.Count > 0)
            {
                ViewBag.Message = message[0];
            }
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
                    ExpiryDate = s.ExpiryDate.ToString("yyyyMMddHHmmss"),
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAsync(EditSkuMainItemViewModel model)
        {
            var Message = new List<string>();
            if (ModelState.IsValid)
            {
                var oldModel = await _skuProductService.GetOne(s => s.ID == model.Id, new List<string>() { "skuSubItems" });
                //var skuSubItems = _skuSubItemService.GetMany(s => s.SkuMainItemId == model.Id, null).ToList();
                if (model.ThumbnailFormFile is not null)
                {
                    FileExtension.DeleteFile(oldModel.ThumbnailImage);
                    oldModel.ThumbnailImage = $"/Uploads/SkuMainItems/{await model.ThumbnailFormFile.CreateFile("SkuMainItems")}";
                }

                oldModel.Name = model.Name;
                oldModel.ShortDescription = model.ShortDescription;
                oldModel.Price = model.Price;
                oldModel.Status = model.Status;
                //oldModel.Quantity = model.Quantity;
                oldModel.CategoryId = model.CategoryId;

                #region
                var addedSubItems = model.ListSkuSubItems.Select(s => new SkuSubItem()
                {
                    BarCodeNumber = s.BarCodeNumber,
                    CreatedDate = DateTime.UtcNow,
                    ExpiryDate = Convert.ToDateTime(s.ExpiryDate),
                    Price = s.Price,
                    SkuMainItemId = oldModel.ID,
                    ModifiedDate = DateTime.UtcNow,
                    Status = SkuItemStatus.available,
                    SkuMainItem = oldModel
                });
                oldModel.skuSubItems.AddRange(addedSubItems);
                //oldModel.skuSubItems = skuSubItems;
                oldModel.Quantity = oldModel.skuSubItems.Count();
                oldModel.Price = oldModel.skuSubItems.OrderByDescending(s => s.Price).First().Price;

                #endregion
                _skuProductService.Update(oldModel);

                Message.Add("Edit");
                return RedirectToAction("Index", new { message = Message });
            }
            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> EditSkuSubItem(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var oldSkuSubItem = _skuSubItemService.GetOne(s => s.ID == id, new List<string>() { "SkuMainItem" }).Result;
            if (oldSkuSubItem == null)
            {
                return NotFound();
            }
            var productViewModel = new EditSkuSubItemViewModel()
            {
                ID = oldSkuSubItem.ID,
                Name = oldSkuSubItem.SkuMainItem.Name,
                Price = oldSkuSubItem.Price,
                BarCodeNumber = oldSkuSubItem.BarCodeNumber,
                ExpiryDate = oldSkuSubItem.ExpiryDate.ToString("yyyy-MM-dd"),
                Status = oldSkuSubItem.Status,
                ThumbnailImage = oldSkuSubItem.SkuMainItem.ThumbnailImage,
                SkuMainItemId = oldSkuSubItem?.SkuMainItemId,
            };

            return View(productViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSkuSubItem(EditSkuSubItemViewModel model)
        {
            var Message = new List<string>();
            if (ModelState.IsValid)
            {
                //var oldModel = await _skuProductService.GetOne(s => s.ID == model.Id, new List<string>() { "skuSubItems" });
                var updateSkuSubItem = _skuSubItemService.GetOne(s => s.ID == model.ID, null).Result;

                updateSkuSubItem.BarCodeNumber = model.BarCodeNumber;
                updateSkuSubItem.Price = model.Price;
                updateSkuSubItem.Status = model.Status;
                updateSkuSubItem.ExpiryDate = Convert.ToDateTime(model.ExpiryDate);

                _skuSubItemService.Update(updateSkuSubItem);

                Message.Add("Edit");
                return RedirectToAction("Edit", new { id = model.SkuMainItemId, message = Message });
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> DeleteSkuSubItem(int id)
        {
            var Message = new List<string>();
            var productSkuSub = await _skuSubItemService.GetOne(s => s.ID == id, null);

            if (productSkuSub is not null)
            {
                await _skuSubItemService.Delete(id);
                //var result = FileExtension.DeleteFile(productSkuSub.ThumbnailImage);
                //if (result.Errors.Count > 0)
                //{
                //    Message.AddRange(result.Errors);
                //}
                //else

                Message.Add("DeleteTrue");
                return RedirectToAction("Edit", new { id = productSkuSub.SkuMainItemId, message = Message });
            }

            return RedirectToAction("Index", new { message = Message });
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var Message = new List<string>();
            var productSkuSub = await _skuProductService.GetOne(s => s.ID == id, null);

            if (productSkuSub is not null)
            {
                productSkuSub.Status= RecordStatus.Deleted;
                _skuProductService.Update(productSkuSub);
                //await _skuSubItemService.Delete(id);
                //var result = FileExtension.DeleteFile(productSkuSub.ThumbnailImage);
                //if (result.Errors.Count > 0)
                //{
                //    Message.AddRange(result.Errors);
                //}
                //else

                Message.Add("DeleteTrue"); return RedirectToAction("Index", new { message = Message });
            }

            return RedirectToAction("Index", new { message = Message });
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
            IEnumerable<SkuSubItem> skuSubItems = _skuSubItemService.GetMany(s => s.SkuMainItemId == id, null)
               .Where(m => string.IsNullOrEmpty(searchValue) ? true : (m.Status.ToString().Contains(searchValue) || m.BarCodeNumber.Contains(searchValue) || m.Price.ToString().Contains(searchValue)));

            var model = skuSubItems.Select(e => new SkuSubItemViewModel()
            {
                ID = e.ID,
                BarCodeNumber = e.BarCodeNumber,
                ExpiryDate = e.ExpiryDate.ToString("yyyy-MM-dd"),
                Price = e.Price,
                Status = e.Status,
            });
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
