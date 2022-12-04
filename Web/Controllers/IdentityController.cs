using Data.Entities.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.Models.UserAuthentication;
using Data.Entities.Enums;

namespace Web.Controllers
{
    public class IdentityController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public IdentityController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            var model = new RegisterViewModel();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if ((await _userManager.FindByEmailAsync(model.Email)) == null)
                {
                    var user = new AppUser()
                    {
                        Email = model.Email,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        UserName = String.Join("", model.FirstName, model.LastName),
                        PhoneNumber = model.PhoneNumber,
                        SecondPhoneNumber = model.SecondPhoneNumber,
                        Age= model.Age,
                        Gender = (Gender)model.GenderId
                    };
                    var result = await _userManager.CreateAsync(user, model.Password);
                    user = await _userManager.FindByEmailAsync(model.Email);

                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    if (result.Succeeded)
                    {
                        Url.ActionLink("ConfirmEmail", "Identity", new { userId = user.Id, @token = token });
                        return RedirectToAction("Login");
                    }
                    ModelState.AddModelError("Register", string.Join("", result.Errors.Select(s => s.Description)));
                    return View(model);
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(RegisterViewModel model)
        {
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var user= await _userManager.FindByNameAsync(userId);
            var result= await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return RedirectToAction("Login");
            }
            return new NotFoundResult();
        }
    }
}
