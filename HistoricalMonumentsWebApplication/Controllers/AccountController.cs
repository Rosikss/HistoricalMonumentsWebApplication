using HistoricalMonumentsWebApplication.Models.DTOs;
using HistoricalMonumentsWebApplication.Models.Enums;
using HistoricalMonumentsWebApplication.Models.IdentityEntities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HistoricalMonumentsWebApplication.Controllers
{
    [Route("[controller]/[action]")]
    //[AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        [Authorize("NotAuthenticated")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Authorize("NotAuthenticated")]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values
                    .SelectMany(temp => temp.Errors)
                    .Select(temp => temp.ErrorMessage);

                return View(registerDTO);
            }

            var user = new ApplicationUser
            {
                Email = registerDTO.Email,
                PersonName = registerDTO.Name,
                PhoneNumber = registerDTO.Phone,
                UserName = registerDTO.Email,
                
            };

            var result = await _userManager.CreateAsync(user, registerDTO.Password);
            if (result.Succeeded)
            {
                if (registerDTO.UserType == UserTypeOptions.Admin)
                {
                    if (await _roleManager.FindByNameAsync(UserTypeOptions.Admin.ToString()) is null)
                    {
                        var applicationRole = new ApplicationRole()
                        {
                            Name = UserTypeOptions.Admin.ToString(),
                        };
                        await _roleManager.CreateAsync(applicationRole);
                    }

                    await _userManager.AddToRoleAsync(user, UserTypeOptions.Admin.ToString());
                }
                else
                {
                    if (await _roleManager.FindByNameAsync(UserTypeOptions.User.ToString()) is null)
                    {
                        var applicationRole = new ApplicationRole()
                        {
                            Name = UserTypeOptions.User.ToString(),
                        };
                        await _roleManager.CreateAsync(applicationRole);
                    }

                    await _userManager.AddToRoleAsync(user, UserTypeOptions.User.ToString());
                }
                await _signInManager.SignInAsync(user, false);
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("Register", error.Description);
            }

            return View(registerDTO);
        }

        [HttpGet]
        [Authorize("NotAuthenticated")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Authorize("NotAuthenticated")]
        public async Task<IActionResult> Login(LoginDTO loginDTO, string? ReturnUrl)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values
                    .SelectMany(temp => temp.Errors)
                    .Select(temp => temp.ErrorMessage);

                return View(loginDTO);
            }

            var result = await _signInManager.PasswordSignInAsync(loginDTO.Email, loginDTO.Password, false, false);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(loginDTO.Email);
                if (user != null)
                {
                    if (await _userManager.IsInRoleAsync(user, UserTypeOptions.Admin.ToString()))
                    {
                        return RedirectToAction("Index", "Architects", new { area = "Admin" });
                    }
                }

                if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                {
                    return LocalRedirect(ReturnUrl);
                }
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            ModelState.AddModelError("Register", "Invalid Email or Password");

            return View(loginDTO);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [AllowAnonymous]
        public async Task<IActionResult> IsEmailAlreadyRegistered(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
            {
                return Json(true);
            }
            return Json(false);
        }
    }
}
