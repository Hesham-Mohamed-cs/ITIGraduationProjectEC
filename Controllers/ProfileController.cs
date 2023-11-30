using GraduaitionProjectITI.Models.Context;
using GraduaitionProjectITI.Models.Entity;
using GraduaitionProjectITI.ViewModel.profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GraduaitionProjectITI.Controllers
{
    public class ProfileController : Controller
    {
        private readonly ECcontext context;
        private readonly UserManager<ApplicationUser> userManager;

        public ProfileController(ECcontext context, UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await userManager.FindByEmailAsync(User.Identity.Name);

            return View(user);
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> EditProfileAsync()
        {
            var userVM = new EditProfileModelView();
            var user = await userManager.FindByEmailAsync(User.Identity.Name);
            userVM.firstName = user.FirstName;
            userVM.lastName = user.LastName;
            userVM.address = user.Adress;
            userVM.phone = user.Phone;
            userVM.email = user.Email;

            return View(userVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditProfileAsync(EditProfileModelView user)
        {
            var found = await userManager.FindByEmailAsync(User.Identity.Name);
            if (ModelState.IsValid)
            {
                if (found != null)
                {
                    found.FirstName = user.firstName;
                    found.LastName = user.lastName;
                    found.Adress = user.address;
                    found.Phone = user.phone;
                    found.Email = user.email;
                    context.SaveChanges();
                    return RedirectToAction("Index","Profile");
                }
            }
            return View(user);
        }


        [Authorize]
        [HttpGet]
        public async Task<ActionResult> OrdersAsync()
        {
            var user = await userManager.FindByEmailAsync(User.Identity.Name);
            List<Order> myOrders = context.orders.Where(o => o.UserId == user.Id).Include(o=>o.subOrders).ToList();

            ViewData["user"] = user;

            return View(myOrders);
        }










        //public async Task<ActionResult> DeleteAsync()
        //{
        //    var found = await userManager.FindByEmailAsync(User.Identity.Name);
        //    return View(found);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> ConfirmDeleteAsync(string id)
        //{
        //    var found = await userManager.FindByEmailAsync(User.Identity.Name);

        //    userManager.DeleteAsync(found);

        //    //context.Users.Remove(found);
        //    //context.SaveChanges();
        //    return RedirectToAction("Logout", "Account");
        //}
    }
}
