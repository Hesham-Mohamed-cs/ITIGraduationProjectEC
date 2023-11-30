using GraduaitionProjectITI.Models.Context;
using GraduaitionProjectITI.Reprository.ProductReprository;
using GraduaitionProjectITI.Reprository.ReviewReprositry;
using GraduaitionProjectITI.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GraduaitionProjectITI.Controllers
{
    [Authorize(Roles = "Admin")]

    public class ReviewController : Controller
    {
        public IReviewReprositry ReviewReprositry { get; }
        public IProductReprository ProductReprository { get; }

        public ReviewController(IReviewReprositry reviewReprositry , IProductReprository productReprository)
        {
            ReviewReprositry = reviewReprositry;
            ProductReprository = productReprository;
        }
        public IActionResult Index()
        {
            return View(ReviewReprositry.GetAllProductThatHaveRevies());
        }

        public IActionResult GetReviews(int Id)
        {
            var product = ProductReprository.GetProduct(Id);
            ViewBag.ProductName = product.Name;
            return View(ReviewReprositry.GetReviewsOnSpecificProduct(product.Id));
        }
    }
}
