using Data.Entities.Enums;
using Data.Entities.Shop;
using Data.Entities.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services.Shop;
using Services.Shop.CategoryRepo;
using System.Net;
using System.Security.Claims;
using Services.Injection;
using System.Web;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json.Linq;
using NuGet.Protocol.Core.Types;

namespace Web.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICartService _cartService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public CartController(IProductService productService, ICartService cartService, RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            _productService = productService;
            _cartService = cartService;
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public IActionResult Index(List<string> message)
        {
            if (message.Count > 0)
            {
                ViewBag.Message = message[0];
            }
            return View();
        }
        public async Task<IActionResult> AddToCart(int id)
        {
            var Message = new List<string>();
            var user = new AppUser();
            //code for user
            var currentUser = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string userName = User.Identity.Name;

            var userAgent = Request.Headers["User-Agent"].ToString();
            string[] subs = userAgent.Split('/');
            //var user1 = _userManager.GetUserAsync(User);
            var userCookieExists = Request.Cookies["Guest_"+ subs[0]];
            if (userName == null)
            {
                //user not signed up and no cookie

                CookieOptions option = new CookieOptions();
                string guestCookie = "Guest_" + subs[0];
                //string userDate = DateTime.UtcNow.ToString();
                //"10.243.2.49"
                string userIPAddress = CommonMethod.GetIPAddress();
                option.Expires = DateTime.Now.AddMonths(1);
                option.IsEssential = true;
                option.Path = "/";
                Response.Cookies.Append(guestCookie, userIPAddress, option);

                AppUser guest = new AppUser()
                {
                    UserName = guestCookie  //this to be used other than the line below
                                           // UserName = "Guest_" + IPAddress + "_" + DateTime.Now.Minute   //this just for test so i can repeat logging in
                };
                var result = await _userManager.CreateAsync(guest, "12345678");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(guest, "Guest");
                    user = await _userManager.FindByNameAsync(guest.UserName);
                    Message.Add("Guest");
                }
                else
                {
                    Message.Add("Error");
                    return Json(Message);
                }

                //userId = string.Join('_', "AnonymousUser", IPAddress);
            }
            else if (userCookieExists!= null)
            {
                // not signed up but has cookie before 
                user = await _userManager.FindByNameAsync(userCookieExists);
            }
            else
            {
                user = await _userManager.FindByIdAsync(currentUser);
            }

            var availableCart = _cartService.GetOne(s => s.AppUserId == user.Id, new List<string> { "ShoppingCartItems" }).Result;
            var product = await _productService.GetOne(s => s.ID == id, null);

            //code for shopping cart
            var shoppingCart = _cartService.AddToShopCart(user, product, availableCart);
            if (availableCart != null && (shoppingCart.StatusOfCompletion == nameof(ShoppingCartStatus.PendingForPreview)))
            {
                _cartService.Update(shoppingCart);
            }
            else
            {
                var cart = _cartService.Insert(shoppingCart);
            }

            return Json(new { message = Message, no = shoppingCart.ShoppingCartItems.Count() });
            //return ViewComponent("CartViewComponent");
        }


    }
}
