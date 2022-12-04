using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ManageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
