using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GraduaitionProjectITI.Controllers
{
    public class HelpController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
