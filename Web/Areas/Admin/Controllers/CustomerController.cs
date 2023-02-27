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

namespace Web.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;
        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        public IActionResult Index(List<string> message)
        {
            if (message.Count > 0)
            {
                ViewBag.Message = message[0];
            }
            var customer = _customerService.GetMany(s => true, null);
            
            var columns = new List<string>()
            {
                "Name",
                "Address",
                "City",
                "PhoneNumber",
                "SecondPhoneNumber",
                "OrdersRequested",
                "CashCollected",
                "Status",
            };
            ViewBag.columns = JsonSerializer.Serialize(columns);
            ViewBag.stringColumns = columns;

            return View();
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
                Address= s.Address,
                City= s.City,
               // Location= s.Location,
                PhoneNumber= s.PhoneNumber,
                SecondPhoneNumber= s.SecondPhoneNumber,
                OrdersRequested= s.OrdersRequested,
                Status = s.Status,
                CashCollected= s.CashCollected,
               
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
