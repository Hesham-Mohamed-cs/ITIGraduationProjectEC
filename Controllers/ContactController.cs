using GraduaitionProjectITI.ViewModel.contact;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;

namespace GraduaitionProjectITI.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Send(ContactViewModel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    MailMessage msz = new MailMessage();
                    msz.From = new MailAddress(vm.Email);//Email which you are getting 
                                                         //from contact us page 
                    msz.To.Add("nm.tt2233@gmail.com");//Where mail will be sent 
                    msz.Subject = vm.Email;
                    msz.Body = vm.Message;
                    SmtpClient smtp = new SmtpClient();

                    smtp.Host = "smtp.gmail.com";

                    smtp.Port = 587;

                    smtp.Credentials = new System.Net.NetworkCredential
                    ("bbnn07633@gmail.com", "bb@12344321");

                    smtp.EnableSsl = true;

                    smtp.Send(msz);

                    return RedirectToAction("Index","Home");

                }
                catch (Exception ex)
                {

                    ModelState.AddModelError("", "Error Happened! Try Another Time !"+ex.Message);
                    return View("Index");

                }
            }


            return View("Index",vm);
        }



    }
}
