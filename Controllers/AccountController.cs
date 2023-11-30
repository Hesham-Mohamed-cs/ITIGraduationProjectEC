using GraduaitionProjectITI.Models.Entity;
using GraduaitionProjectITI.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GraduaitionProjectITI.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInMAnager;

        public AccountController
            (UserManager<ApplicationUser> _UserManager,
            SignInManager<ApplicationUser> _SignInMAnager)
        {
            userManager = _UserManager;
            signInMAnager = _SignInMAnager;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel newUserVM)
        {
            if (ModelState.IsValid)
            {
                //create account
                ApplicationUser userModel = new ApplicationUser();
                userModel.Email = newUserVM.Email;
                userModel.Adress = newUserVM.Adress;
                userModel.Phone = newUserVM.Phone;
                userModel.FirstName = newUserVM.FirstName;
                userModel.LastName = newUserVM.LastName;
                userModel.UserName = newUserVM.Email;
               // userModel.Password = newUserVM.Password;
                IdentityResult result = await userManager.CreateAsync(userModel, newUserVM.Password);
                if (result.Succeeded == true)
                {
                   await signInMAnager.SignInAsync(userModel, false);
                   await userManager.AddToRoleAsync(userModel, "User");
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }

            }
            return View(newUserVM);
        }


        [HttpGet]
        public IActionResult Login(string ReturnUrl="~/home/index")
        {
            TempData["ReturnUrl"] = ReturnUrl;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel UserVm )
        {
            if (ModelState.IsValid)
            {
                //check
                var userModel = await userManager.FindByEmailAsync(UserVm.Email);
                if (userModel != null)
                {
                    var x = await signInMAnager.PasswordSignInAsync(userModel, UserVm.Password, UserVm.RememberMe, false);
                    if (x.Succeeded == true)
                    {
                        return LocalRedirect(TempData.ContainsKey("ReturnUrl") ?TempData["ReturnUrl"].ToString():"~/home/index");
                    }
                    ModelState.AddModelError("", "Username and password invalid");

                }
                ModelState.AddModelError("", "Username and password invalid");
            }
            return View(UserVm);
        }

        public async Task<IActionResult> Logout()
        {
            await signInMAnager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

    }
}
