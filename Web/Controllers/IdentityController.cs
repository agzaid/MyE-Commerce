using Data.Entities.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.Models.UserAuthentication;
using Data.Entities.Enums;
using Services.Injection.CommonResult;

namespace Web.Controllers
{
    public class IdentityController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public IdentityController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            var model = new RegisterViewModel() { Role = "member" };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (!await _roleManager.RoleExistsAsync(model.Role))
                {
                    var role = new IdentityRole { Name = model.Role };
                    var roleResult = await _roleManager.CreateAsync(role);
                    if (!roleResult.Succeeded)
                    {
                        var errors = roleResult.Errors.Select(s => s.Description);
                        ModelState.AddModelError("Role", string.Join(", ", errors));
                        return View(model);
                    }
                }
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
                        Age = model.Age,
                        Gender = (Gender)model.GenderId
                    };
                    var result = await _userManager.CreateAsync(user, model.Password);
                    user = await _userManager.FindByEmailAsync(model.Email);

                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    if (result.Succeeded)
                    {
                        if (model.Role.Contains("Admin"))
                        {
                            await _userManager.AddToRoleAsync(user, "Admin");
                        }
                        else
                        {
                            await _userManager.AddToRoleAsync(user, model.Role);
                        }
                        //await _userManager.AddToRoleAsync(user, "Admin");
                        //Url.ActionLink("ConfirmEmail", "Identity", new { userId = user.Id, @token = token });
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
            return View(new LoginViewModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                try
                {
                    var user = await _userManager.FindByNameAsync(model.UserName);
                    if (user == null)
                    {
                        user = await _userManager.FindByEmailAsync(model.UserName);
                    }
                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: false);
                    if (result.Succeeded)
                    {
                        TempData["Message"] = "Success";
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        TempData["Message"] = "Failed";
                        ModelState.AddModelError("Login", "Login Failed");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Login", string.Join("", ex.InnerException));
                    return RedirectToAction("Index", "Home");
                    throw;
                }
            }
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            TempData["Message"] = "Logout";

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var user = await _userManager.FindByNameAsync(userId);
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return RedirectToAction("Login");
            }
            return new NotFoundResult();
        }
    }
}
