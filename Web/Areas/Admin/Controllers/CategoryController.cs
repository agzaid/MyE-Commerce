using AutoMapper;
using Data.Entities.Shop;
using Microsoft.AspNetCore.Mvc;
using Services.Injection;
using Services.Shop.CategoryRepo;
using System.Text.Json;
using Web.Areas.Admin.Models.Shop.CategoryVM;

namespace Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        public CategoryController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }
        public IActionResult Index(List<string> message)
        {
            if (message.Count > 0)
            {
                ViewBag.Message = message[0];
            }
            var categories = _categoryService.GetMany(s => true, null);

            var columns = new List<string>()
            {
                "Name",
                "DisplayOrder",
                "Status",
            };
            ViewBag.columns = JsonSerializer.Serialize(columns);
            ViewBag.stringColumns = columns;

            return View(categories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var category = new CreateCategoryViewModel();
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCategoryViewModel model)
        {
            var Message = new List<string>();
            if (ModelState.IsValid)
            {
                var category = new Category()
                {
                    ThumbnailImage = $"/Uploads/Categories/{await model.ThumbnailFormFile.CreateFile("Categories")}",
                    CreatedDate = DateTime.Now,
                    DisplayOrder = model.DisplayOrder,
                    ModifiedDate = DateTime.Now,
                    Name = model.CategoryName,
                    Status = model.Status,
                };
                _categoryService.Insert(category);
                Message.Add("Create");
            }
            return RedirectToAction("Index", new { message = Message });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var category = await _categoryService.GetOne(s => s.ID == id, null);
            var categoryViewModel = _mapper.Map<CreateCategoryViewModel>(category);

            if (category == null)
            {
                return NotFound();
            }
            return View(categoryViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAsync(CreateCategoryViewModel model)
        {
            var Message = new List<string>();
            if (ModelState.IsValid)
            {
                var oldModel = await _categoryService.GetOne(s => s.ID == model.ID, null);

                if (model.ThumbnailFormFile is not null)
                {
                    FileExtension.DeleteFile(oldModel.ThumbnailImage);
                    oldModel.ThumbnailImage = $"/Uploads/Categories/{await model.ThumbnailFormFile.CreateFile("Categories")}";
                }

                oldModel.Name = model.CategoryName;
                oldModel.Status = model.Status;
                oldModel.DisplayOrder = model.DisplayOrder;

                _categoryService.Update(oldModel);

                Message.Add("Edited");
                return RedirectToAction("Index", new { message = Message });
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var Message = new List<string>();
            try
            {
                var category = await _categoryService.GetOne(s => s.ID == id, new List<string> { "Products" });
                if (category is not null)
                {
                    await _categoryService.Delete(id);
                    var result = FileExtension.DeleteFile(category.ThumbnailImage);
                    if (result.Errors.Count > 0)
                    {
                        Message.AddRange(result.Errors);
                    }
                    else
                        Message.Add("DeleteTrue");

                    return RedirectToAction("Index", new { message = Message });
                }

            }
            catch (Exception ex)
            {
                Message.Add(ex.Message);
                throw;
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
            IEnumerable<Category> categories = _categoryService.GetMany(s => true, null)
                .Where(m => string.IsNullOrEmpty(searchValue) ? true : (m.Name.Contains(searchValue)));

            //for sorting
            //IQueryable<Product> queryProducts = (IQueryable<Product>)products;
            //if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortDir)))
            //   products = queryProducts.OrderBy(string.Concat(sortColumn, " ", sortDir));

            var data = categories.Skip(skip).Take(pageSize).ToList();

            var recordsTotal = categories.Count();
            ViewData["records"] = recordsTotal;
            return Ok(new { recordsFiltered = recordsTotal, recordsTotal, data = data });
        }

        #endregion
    }
}
