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

        public CartController(IProductService productService, ICartService cartService)
        {
            _productService = productService;
            _cartService = cartService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> AddToCart(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                //"10.243.2.49"
                string IPAddress = CommonMethod.GetIPAddress();
                userId = string.Join('_', "AnonymousUser", IPAddress);
            }

            var product = await _productService.GetOne(s => s.ID == id, null);
            var shoppingCart = new ShoppingCart()
            {
                AppUserId = userId,
                StatusOfCompletion = ShoppingCartStatus.PendingForPreview.ToString(),
                CreatedDate= DateTime.UtcNow,
                ModifiedDate= DateTime.UtcNow,
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

             _cartService.Insert(shoppingCart);

            return ViewComponent("MyViewComponent");
        }

       
    }
}
