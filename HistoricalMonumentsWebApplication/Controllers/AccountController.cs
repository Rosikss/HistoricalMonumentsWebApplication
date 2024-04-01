using HistoricalMonumentsWebApplication.Models;
using HistoricalMonumentsWebApplication.Models.DTOs;
using HistoricalMonumentsWebApplication.Models.Enums;
using HistoricalMonumentsWebApplication.Models.IdentityEntities;
using HistoricalMonumentsWebApplication.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.Text.Encodings.Web;

namespace HistoricalMonumentsWebApplication.Controllers
{
    [Route("[controller]/[action]")]
    //[AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IEmailSender _emailSender;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
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

                    await _signInManager.SignInAsync(user, false);

                    return RedirectToAction("Index", "Architects", new { area = "Admin" });
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

                    await SendConfirmationEmail(user);
                }
                
                return RedirectToAction("RegisterSuccessful", "Account");
            }
        
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("Register", error.Description);
            }

            return View(registerDTO);
        }

        [HttpGet]
        [Authorize("NotAuthenticated")]
        public IActionResult RegisterSuccessful()
        {
            return View();
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

            var user = await _userManager.FindByEmailAsync(loginDTO.Email);

            if (user != null)
            {
                if (!await _userManager.IsInRoleAsync(user, UserTypeOptions.Admin.ToString()) && !user.EmailConfirmed)
                {
                    await SendConfirmationEmail(user);

                    return RedirectToAction("RegisterSuccessful", "Account");
                }
                
                var result = await _signInManager.PasswordSignInAsync(user, loginDTO.Password, false, false);
                
                if (result.Succeeded)
                {
                    if (await _userManager.IsInRoleAsync(user, UserTypeOptions.Admin.ToString()))
                    {
                        return RedirectToAction("Index", "Architects", new { area = "Admin" });
                    }

                    if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                    {
                        return LocalRedirect(ReturnUrl);
                    }

                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }
            }

            ModelState.AddModelError("Register", "Неправильна пошта чи пароль");

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

        [Authorize("NotAuthenticated")]
        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }
            }

            return RedirectToAction("Register", "Account");
        }

        [HttpGet]
        [Authorize("NotAuthenticated")]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [Authorize("NotAuthenticated")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDTO model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null && await _userManager.IsEmailConfirmedAsync(user) || await _userManager.IsInRoleAsync(user, UserTypeOptions.Admin.ToString()))
                {
                    await SendForgotPasswordEmail(user);
                    return RedirectToAction("ForgotPasswordConfirmation", "Account");
                }
                
                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }
            return View(model);
        }

        [HttpGet]
        [Authorize("NotAuthenticated")]
        public IActionResult ResetPassword(string Token, string Email)
        {
            if (Token == null || Email == null)
            {
                ViewBag.ErrorTitle = "Invalid Password Reset Token";
                ViewBag.ErrorMessage = "The Link is Expired or Invalid";
                return View("Error");
            }
            
            ResetPasswordDTO model = new ResetPasswordDTO();
            model.Token = Token;
            model.Email = Email;
            return View(model);
        }

        [HttpPost]
        [Authorize("NotAuthenticated")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("ResetPasswordConfirmation", "Account");
                    }
                    
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(model);
                }
                
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            
            return View(model);
        }

        [Authorize("NotAuthenticated")]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        [Authorize("NotAuthenticated")]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }
        private async Task SendConfirmationEmail(ApplicationUser? user)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            if (!string.IsNullOrEmpty(token))
            {
                var confirmationLink = Url.Action("ConfirmEmail", "Account", new { token, email = user.Email }, Request.Scheme);
                var message = new Message(new string[] { user.Email! }, "Підтвердження пошти", confirmationLink!);
                _emailSender.SendEmail(message);

                ///////////////////////////////////////////////////////////////////////ViewBag.Favourites = (await _userManager.FindByNameAsync(User.Identity.Name)).FavouriteHistoricalMonuments;
                
            }
        }
        private async Task SendForgotPasswordEmail(ApplicationUser? user)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            if (!string.IsNullOrEmpty(token))
            {
                var confirmationLink = Url.Action("ResetPassword", "Account", new { token, email = user.Email }, Request.Scheme);
                var message = new Message(new string[] { user.Email! }, "Скидання паролю", confirmationLink!);
                _emailSender.SendEmail(message);
            }
        }
    }
}
