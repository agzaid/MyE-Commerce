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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Web.Controllers
{
    public class CartController : BaseController<CartController>
    {
        public List<string> Message = new List<string>();
        private readonly IProductService _productService;
        private readonly ICartService _cartService;


        public CartController(IProductService productService, ICartService cartService)
        {
            _productService = productService;
            _cartService = cartService;
        }
        public async Task<IActionResult> Index(List<string> message)
        {
                ShoppingCart availableCart = new();
            try
            {
                if (message.Count > 0)
                {
                    ViewBag.Message = message[0];
                }
                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await UserManager.FindByIdAsync(currentUserId);
                if (user != null)
                {
                    availableCart = _cartService.GetOne(s => s.AppUserId == user.Id, new List<string> { "ShoppingCartItems" }).Result;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
           

            return View(availableCart);
        }
        public async Task<IActionResult> AddToCart(int id)
        {
            try
            {
                //var Message = new List<string>();
                var user = new AppUser();
                //code for user
                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                string currentUserName = User.Identity.Name;

                //user = await Cookie(currentUserName, user, currentUserId);

                #region cookie
                var userAgent = Request.Headers["User-Agent"].ToString();
                //"10.243.2.49"
                //"192.168.137.1"
                string userIPAddress = CommonMethod.GetIPAddress();
                string[] subs = userAgent.Split('/');
                var userCookieExists = Request.Cookies["Guest"];
                if (currentUserName == null)
                {
                    //user not signed up and no cookie
                    CookieOptions option = new CookieOptions();
                    string guestCookie = "Guest";
                    option.Expires = DateTime.Now.AddMonths(1);
                    option.IsEssential = true;
                    option.Path = "/";
                    AppUser guest = new AppUser()
                    {
                        UserName = guestCookie
                    };
                    var result = await UserManager.CreateAsync(guest, "12345678");
                    if (result.Succeeded)
                    {
                        await UserManager.AddToRoleAsync(guest, "Guest");
                        user = await UserManager.FindByNameAsync(guest.UserName);
                        Response.Cookies.Append(guestCookie, user.Id, option);
                        //signing in user
                        if (!User.Identity.IsAuthenticated)
                            await SignInManager.PasswordSignInAsync(user, "12345678", false, lockoutOnFailure: false);
                        Message.Add("Guest");
                    }
                    else
                    {
                        Message.Add("Error");
                        return Json(Message);
                    }
                }
                else if (userCookieExists != null && currentUserName.Contains("Guest"))
                {
                    // not signed up but has cookie before 
                    user = await UserManager.FindByIdAsync(userCookieExists);
                    if (!User.Identity.IsAuthenticated)
                        await SignInManager.PasswordSignInAsync(user, "12345678", false, lockoutOnFailure: false);
                }
                else
                {
                    user = await UserManager.FindByIdAsync(currentUserId);
                }
                #endregion

                var availableCart = _cartService.GetOne(s => s.AppUserId == user.Id, new List<string> { "ShoppingCartItems" }).Result;
                var product = await _productService.GetOne(s => s.ID == id, null);

                //code for shopping cart
                var shoppingCart = _cartService.AddToShopCart(user, product, availableCart);
                if (availableCart != null && (shoppingCart.StatusOfCompletion == nameof(ShoppingCartStatus.PendingForPreview)))
                {
                    _cartService.Update(shoppingCart);
                    Message.Add("Item Added");
                }
                else
                {
                    var cart = _cartService.Insert(shoppingCart);
                    Message.Add("Item Added");
                }

                return Json(new { message = Message, no = shoppingCart.ShoppingCartItems.Count() });
                //return ViewComponent("CartViewComponent");
            }
            catch (Exception ex)
            {

                throw;
            }

        }


        public async Task<AppUser> Cookie(string currentUserName, AppUser user, string currentUserId)
        {

            var userAgent = Request.Headers["User-Agent"].ToString();
            //"10.243.2.49"
            //"192.168.137.1"
            string userIPAddress = CommonMethod.GetIPAddress();
            string[] subs = userAgent.Split('/');
            var userCookieExists = Request.Cookies["Guest_" + subs[0]];
            if (currentUserName == null)
            {
                //user not signed up and no cookie
                CookieOptions option = new CookieOptions();
                string guestCookie = "Guest_" + subs[0];
                option.Expires = DateTime.Now.AddMonths(1);
                option.IsEssential = true;
                option.Path = "/";
                AppUser guest = new AppUser()
                {
                    UserName = guestCookie
                };
                var result = await UserManager.CreateAsync(guest, "12345678");
                if (result.Succeeded)
                {
                    await UserManager.AddToRoleAsync(guest, "Guest");
                    user = await UserManager.FindByNameAsync(guest.UserName);
                    Response.Cookies.Append(guestCookie, user.Id, option);
                    //signing in user
                    var signInCookie = await SignInManager.PasswordSignInAsync(user, "12345678", false, lockoutOnFailure: false);
                    Message.Add("Guest");
                }
                else
                {
                    Message.Add("Error");
                    //return Json(Message);
                }

                //userId = string.Join('_', "AnonymousUser", IPAddress);
            }
            else if (userCookieExists != null && currentUserName.Contains("Guest_"))
            {
                // not signed up but has cookie before 
                user = await UserManager.FindByIdAsync(userCookieExists);
                var signInCookie = await SignInManager.PasswordSignInAsync(user, "12345678", false, lockoutOnFailure: false);
            }
            else
            {
                user = await UserManager.FindByIdAsync(currentUserId);
            }
            return user;
        }


    }
}
