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
    public class CartController : Controller
    {
        public List<string> Message = new List<string>();
        private readonly IProductService _productService;
        private readonly ICartService _cartService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public CartController(IProductService productService, ICartService cartService, RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
        {
            _productService = productService;
            _cartService = cartService;
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
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
                    var result = await _userManager.CreateAsync(guest, "12345678");
                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(guest, "Guest");
                        user = await _userManager.FindByNameAsync(guest.UserName);
                        Response.Cookies.Append(guestCookie, user.Id, option);
                        //signing in user
                        var signInCookie = await _signInManager.PasswordSignInAsync(user, "12345678", false, lockoutOnFailure: false);
                        Message.Add("Guest");
                    }
                    else
                    {
                        Message.Add("Error");
                        return Json(Message);
                    }

                    //userId = string.Join('_', "AnonymousUser", IPAddress);
                }
                else if (userCookieExists != null && currentUserName.Contains("Guest_"))
                {
                    // not signed up but has cookie before 
                    user = await _userManager.FindByIdAsync(userCookieExists);
                    var signInCookie = await _signInManager.PasswordSignInAsync(user, "12345678", false, lockoutOnFailure: false);
                }
                else
                {
                    user = await _userManager.FindByIdAsync(currentUserId);
                }
                #endregion

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
            catch (Exception ex)
            {

                throw;
            }

        }


        public async Task<AppUser> Cookie(string currentUserName,AppUser user, string currentUserId)
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
                var result = await _userManager.CreateAsync(guest, "12345678");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(guest, "Guest");
                    user = await _userManager.FindByNameAsync(guest.UserName);
                    Response.Cookies.Append(guestCookie, user.Id, option);
                    //signing in user
                    var signInCookie = await _signInManager.PasswordSignInAsync(user, "12345678", false, lockoutOnFailure: false);
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
                user = await _userManager.FindByIdAsync(userCookieExists);
                var signInCookie = await _signInManager.PasswordSignInAsync(user, "12345678", false, lockoutOnFailure: false);
            }
            else
            {
                user = await _userManager.FindByIdAsync(currentUserId);
            }
            return user;
        }


    }
}
