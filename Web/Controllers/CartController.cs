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

            var currentUser = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUser == null)
            {
                //"10.243.2.49"
                string IPAddress = CommonMethod.GetIPAddress();
                AppUser guest = new AppUser()
                {
                    UserName = "Guest_" + IPAddress
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
            }else
                user = await _userManager.FindByNameAsync(currentUser);

            var product = await _productService.GetOne(s => s.ID == id, null);
            var shoppingCart = new ShoppingCart()
            {
                AppUser = user,
                AppUserId = user.Id,
                StatusOfCompletion = ShoppingCartStatus.PendingForPreview.ToString(),
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
                ShoppingCartItems = new List<ShoppingCartItem>(),
            };
            shoppingCart.ShoppingCartItems.Add(new ShoppingCartItem
            {
                Product = product,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
                Quantity = 1,
                ProductID = id
            });

            var cart = _cartService.Insert(shoppingCart);

            return Json(Message);
            //return ViewComponent("MyViewComponent");
        }


    }
}
