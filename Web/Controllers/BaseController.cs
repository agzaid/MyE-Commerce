using Data.Entities.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class BaseController<T> : Controller where T : BaseController<T>
    {
        private ILogger<T>? _logger;
        private UserManager<AppUser> _userManager;
        private SignInManager<AppUser> _signInManager;
        private RoleManager<IdentityRole> _roleManager;


        protected ILogger<T> Logger
            =>_logger??= HttpContext.RequestServices.GetRequiredService<ILogger<T>>();
        protected UserManager<AppUser> UserManager
            =>_userManager??= HttpContext.RequestServices.GetRequiredService<UserManager<AppUser>>();
        protected SignInManager<AppUser> SignInManager
            =>_signInManager??= HttpContext.RequestServices.GetRequiredService<SignInManager<AppUser>>();
        protected RoleManager<IdentityRole> RoleManager
            =>_roleManager??= HttpContext.RequestServices.GetRequiredService<RoleManager<IdentityRole>>();
    }
}
