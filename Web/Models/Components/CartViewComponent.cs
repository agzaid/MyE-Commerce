using Data.Entities.Enums;
using Microsoft.AspNetCore.Mvc;
using Services.Shop;
using Services.Shop.CategoryRepo;

namespace Web.Models.Components
{
    public class CartViewComponent : ViewComponent
    {
        private readonly ICartService _cartService;
        public CartViewComponent(ICartService cartService)
        {
            _cartService = cartService;
        }
        public IViewComponentResult Invoke()
        {
            //var user= UserClaimsPrincipal.Identity.ToString();
            var cart = _cartService.GetMany(s => true, null).Count();
            return View(cart);
        }

    }

}

