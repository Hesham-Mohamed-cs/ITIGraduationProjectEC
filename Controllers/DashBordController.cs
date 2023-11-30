using GraduaitionProjectITI.Reprository.BrandRepository;
using GraduaitionProjectITI.Reprository.CategoryReprositry;
using GraduaitionProjectITI.Reprository.OrderReprository;
using GraduaitionProjectITI.Reprository.ProductReprository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GraduaitionProjectITI.Controllers
{
    [Authorize(Roles ="Admin")]
    public class DashBordController : Controller
    {
        private readonly IProductReprository productReprository;
        private readonly ICategoryReprositry categoryReprositry;
        private readonly IBrandRepository brandRepository;
        private readonly IOrderReprository orderReprository;

        public DashBordController(IProductReprository productReprository,
            ICategoryReprositry categoryReprositry, IBrandRepository brandRepository
            , IOrderReprository orderReprository)
        {
            this.productReprository = productReprository;
            this.categoryReprositry = categoryReprositry;
            this.brandRepository = brandRepository;
            this.orderReprository = orderReprository;
        }
        public IActionResult Index()
        {
            ViewBag.productCount = productReprository.GetAll().Count();
            ViewBag.CategoryCount = categoryReprositry.GetAll().Count();
            ViewBag.BrandCount = brandRepository.GetAll().Count();
            ViewBag.orderCount = orderReprository.GetAll().Count();
            return View();
        }
    }
}
