using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Web.Models;

namespace Web.Controllers
{
    public class HomeController : BaseController<HomeController>
    {

        public async Task<IActionResult> Index(List<string> message)
        {
            var userCookieExists = Request.Cookies["Guest"];
            var user = await UserManager.FindByIdAsync(userCookieExists);
            await SignInManager.PasswordSignInAsync(user, "12345678", false, lockoutOnFailure: false);


            if (message.Count > 0)
            {
                TempData["Message"] = message[0];
            }
            return View(user);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}