using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Web.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class CustomerController : Controller
    {
        public CustomerController()
        {

        }
        public IActionResult Index(List<string> message)
        {
            if (message.Count > 0)
            {
                ViewBag.Message = message[0];
            }
            //var products = _productService.GetMany(s => true, new List<string>() { "Category" });

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

            return View();
        }
    }
}
