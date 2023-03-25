using Data.Entities.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Web.Models.Components
{
    public class UserIdentityViewComponent : ViewComponent
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserIdentityViewComponent(UserManager<AppUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var currentUser = _userManager.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).ToString();
            var user = await _userManager.FindByNameAsync(currentUser);

            return View(user);
        }
    }
}
