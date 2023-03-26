using Data.Entities.Enums;
using Data.Entities.Shop;
using Data.Entities.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services.Shop;
using Services.Shop.CategoryRepo;

namespace Web.Models.Components
{
    public class CartViewComponent : ViewComponent
    {
        private readonly ICartService _cartService;
        private readonly UserManager<AppUser> _userManager;

        public CartViewComponent(ICartService cartService, UserManager<AppUser> userManager)
        {
            _cartService = cartService;
            _userManager = userManager;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            AppUser user = new();
            var userCookieExists = Request.Cookies["Guest"];
            if (userCookieExists != null)
            {
                user = await _userManager.FindByIdAsync(userCookieExists);
            }
            else
            {
                var userClaim = UserClaimsPrincipal.Identity.ToString();
                user = await _userManager.FindByNameAsync(userClaim);
            }
            var cart = 0;
            if (user != null)
            {
                var shopCart = _cartService.GetOne(s => s.AppUserId == user.Id && s.StatusOfCompletion == ShoppingCartStatus.PendingForPreview.ToString(), null).Result;
                cart = shopCart.ShoppingCartItems.Count;
            }

            //var  cart = _cartService.GetMany(s => true, null).Count();

            return View(cart);
        }

    }

}

