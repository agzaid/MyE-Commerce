using AutoMapper;
using Data.Entities.Shop;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Services.Shop.CategoryRepo;
using Services.Shop;
using System.Text.Json;
using System.Xml.Linq;
using Web.Areas.Admin.Models.Shop.product;
using Services.Shop.CustomerRepo;
using Data.Entities.User;
using Web.Areas.Admin.Models.Users;
using Data.Entities.Enums;
using Web.Areas.Admin.Models.Shop;
using Services.Injection;

namespace Web.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;

        public CustomerController(ICustomerService customerService, IMapper mapper)
        {
            _customerService = customerService;
            _mapper = mapper;
        }
        public IActionResult Index(List<string> message)
        {
            if (message.Count > 0)
            {
                ViewBag.Message = message[0];
            }
            //var customer = _customerService.GetMany(s => true, null);

            var columns = new List<string>()
            {
                "Name",
                "Address",
                "City",
                "PhoneNumber",
                //"SecondPhoneNumber",
                "OrdersRequested",
                "CashCollected",
                "Status",
            };
            ViewBag.columns = JsonSerializer.Serialize(columns);
            ViewBag.stringColumns = columns;

            return View();
        }
        [HttpGet]
        public IActionResult Create()
        {
            var customer = new IndexCustomersViewModel();

            customer.Status = RecordStatus.Published;
            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync(IndexCustomersViewModel model)
        {
            var Message = new List<string>();

            if (ModelState.IsValid)
            {
                var customer = new Customer()
                {
                    Name = model.Name,
                    Address = model.Address,
                    CashCollected = model.CashCollected,
                    City = model.City,
                    Location = model.Location,
                    OrdersRequested = model.OrdersRequested,
                    PhoneNumber = model.PhoneNumber,
                    SecondPhoneNumber = model.SecondPhoneNumber,
                    Status = model.Status,
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow
                };
                _customerService.Insert(customer);
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

            var model = new IndexCustomersViewModel();
            var customer = await _customerService.GetOne(s => s.ID == id, null);
            if (customer == null)
            {
                return NotFound();
            }

            var customerViewModel = _mapper.Map<IndexCustomersViewModel>(customer);

            return View(customerViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAsync(IndexCustomersViewModel model)
        {
            var Message = new List<string>();
            if (ModelState.IsValid)
            {
                var oldModel = await _customerService.GetOne(s => s.ID == model.ID, null);
                if (oldModel == null) { return NotFound(); }

                var customerViewModel = _mapper.Map<Customer>(oldModel);
                customerViewModel.Name = model.Name;
                customerViewModel.Status = model.Status;
                customerViewModel.PhoneNumber = model.PhoneNumber;
                customerViewModel.SecondPhoneNumber = model.SecondPhoneNumber;
                customerViewModel.Address = model.Address;
                customerViewModel.City = model.City;
                customerViewModel.CashCollected = model.CashCollected;
                customerViewModel.Location = model.Location;
                customerViewModel.ModifiedDate = DateTime.UtcNow;

                _customerService.Update(customerViewModel);

                Message.Add("Edited");
                return RedirectToAction("Index", new { message = Message });
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var Message = new List<string>();
            var customer = await _customerService.GetOne(s => s.ID == id, null);

            if (customer is not null)
            {
                //await _customerService.Delete(id);

                customer.Status = RecordStatus.Deleted;
                _customerService.Update(customer);

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
            IEnumerable<Customer> customers = _customerService.GetMany(s => true, null)
                .Where(m => string.IsNullOrEmpty(searchValue) ? true : (m.Name.Contains(searchValue) || m.Address.Contains(searchValue) || m.City.ToString().Contains(searchValue)));

            var model = customers.Select(s => new IndexCustomersViewModel()
            {
                ID = s.ID,
                Name = s.Name,
                Address = s.Address,
                City = s.City,
                // Location= s.Location,
                PhoneNumber = s.PhoneNumber + "," + s.SecondPhoneNumber,
                //SecondPhoneNumber = s.SecondPhoneNumber,
                OrdersRequested = s.OrdersRequested,
                Status = s.Status,
                CashCollected = s.CashCollected,

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
