using GraduaitionProjectITI.Models.Context;
using GraduaitionProjectITI.Reprository.BrandRepository;
using GraduaitionProjectITI.Reprository.CategoryReprositry;
using GraduaitionProjectITI.Reprository.OrderReprository;
using GraduaitionProjectITI.Reprository.ProductReprository;
using Microsoft.AspNetCore.Mvc;
namespace GraduaitionProjectITI.Controllers
{
    public class ReportController : Controller
    {
        private readonly ECcontext context;
        private readonly IProductReprository productReprository;
        private readonly ICategoryReprositry categoryReprositry;
        private readonly IBrandRepository brandRepository;
        private readonly IOrderReprository orderReprository;

        public ReportController(ECcontext context, IProductReprository productReprository,
            ICategoryReprositry categoryReprositry, IBrandRepository brandRepository
            , IOrderReprository orderReprository)
        {
            this.context = context;
            this.productReprository = productReprository;
            this.categoryReprositry = categoryReprositry;
            this.brandRepository = brandRepository;
            this.orderReprository = orderReprository;
        }
        public IActionResult Index()
        {
            var orderreport = context.orders.ToList();
            ViewBag.productCount = productReprository.GetAll().Count();
            ViewBag.CategoryCount = categoryReprositry.GetAll().Count();
            ViewBag.BrandCount = brandRepository.GetAll().Count();
            ViewBag.orderCount = orderReprository.GetAll().Count();
            return View(orderreport);
        }
    }
}
