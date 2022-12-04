using AutoMapper;
using Data.Entities.Shop;
using Microsoft.AspNetCore.Mvc;
using Services.Injection;
using Services.Shop;
using Web.Models.Shop;
using System.Linq.Dynamic.Core;
using System.Linq;
using Web.Areas.Admin.Models.Shop;
using System.Text.Json;
using Web.Areas.Admin.Models.Shop.product;
using Services.Shop.CategoryRepo;

namespace Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        private readonly ICategoryService _categoryService;

        public ProductController(IProductService productService, IMapper mapper, ICategoryService categoryService)
        {
            _productService = productService;
            _mapper = mapper;
            _categoryService = categoryService;
        }

        public IActionResult Index(List<string> message)
        {
            if (message.Count > 0)
            {
                ViewBag.Message = message[0];
            }
            var products = _productService.GetMany(s => true, new List<string>() { "Category" });



            var columns = new List<string>()
            {
                "Name",
                "Price",
                "DisplayOrder",
                "Quantity",
                "ShortDescription",
                "Status"
            };
            ViewBag.columns = JsonSerializer.Serialize(columns);
            ViewBag.stringColumns = columns;

            return View(products);
        }


        [HttpGet]
        public IActionResult Create()
        {
            var product = new CreateProductViewModel();
            var categories = _categoryService.GetMany(s => true, null);
            product.ListOfCategories = categories.Select(s => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem()
            {
                Text = s.Name,
                Value = s.ID.ToString(),
            }).ToList();
            return View(product);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync(CreateProductViewModel model)
        {
            var Message = new List<string>();
            if (ModelState.IsValid)
            {
                var product = new Product()
                {
                    ThumbnailImage = $"/Uploads/Products/{await model.ThumbnailFormFile.CreateFile("Products")}",
                    CreatedDate = DateTime.UtcNow,
                    DisplayOrder = model.DisplayOrder,
                    ModifiedDate = DateTime.UtcNow,
                    Price = model.Price,
                    ProductName = model.ProductName,
                    Quantity = model.Quantity,
                    ShortDescription = model.ShortDescription,
                    Status = model.Status,
                };
                _productService.Insert(product);
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

            var model = new CreateProductViewModel();
            var product = await _productService.GetOne(s => s.ID == id, null);
            var categories = _categoryService.GetMany(s => true, null);
            var productViewModel = _mapper.Map<CreateProductViewModel>(product);
            productViewModel.ListOfCategories = categories.Select(s => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem()
            {
                Text = s.Name,
                Value = s.ID.ToString(),
            }).ToList();

            if (product == null)
            {
                return NotFound();
            }
            return View(productViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAsync(CreateProductViewModel model)
        {
            var Message = new List<string>();
            if (ModelState.IsValid)
            {
                var oldModel = await _productService.GetOne(s => s.ID == model.ID, null);

                if (model.ThumbnailFormFile is not null)
                {
                    FileExtension.DeleteFile(oldModel.ThumbnailImage);
                    oldModel.ThumbnailImage = $"/Uploads/Products/{await model.ThumbnailFormFile.CreateFile("Products")}";
                }

                oldModel.ProductName = model.ProductName;
                oldModel.ShortDescription = model.ShortDescription;
                oldModel.Price = model.Price;
                oldModel.Status = model.Status;
                oldModel.Quantity = model.Quantity;
                oldModel.DisplayOrder = model.DisplayOrder;
                oldModel.CategoryId= model.CategoryId;

                _productService.Update(oldModel);

                Message.Add("Edited");
                return RedirectToAction("Index", new { message = Message });
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var Message = new List<string>();
            var product = await _productService.GetOne(s => s.ID == id, null);

            if (product is not null)
            {
                await _productService.Delete(id);
                var result = FileExtension.DeleteFile(product.ThumbnailImage);
                if (result.Errors.Count > 0)
                {
                    Message.AddRange(result.Errors);
                }
                else
                    Message.Add("DeleteTrue");

                return RedirectToAction("Index", new { message = Message });
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
            IEnumerable<Product> products = _productService.GetMany(s => true, null)
                .Where(m => string.IsNullOrEmpty(searchValue) ? true : (m.ProductName.Contains(searchValue) || m.ShortDescription.Contains(searchValue) || m.Price.ToString().Contains(searchValue)));

            var model = products.Select(s => new IndexProductViewModel()
            {
                ID = s.ID,
                Name = s.ProductName,
                Price = s.Price,
                DisplayOrder = s.DisplayOrder,
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

        #endregion


    }
}
