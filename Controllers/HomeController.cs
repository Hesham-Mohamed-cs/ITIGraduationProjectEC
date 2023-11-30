using GraduaitionProjectITI.Models;
using GraduaitionProjectITI.Models.Entity;
using GraduaitionProjectITI.Reprository.BrandRepository;
using GraduaitionProjectITI.Reprository.CategoryReprositry;
using GraduaitionProjectITI.Reprository.ProductReprository;
using GraduaitionProjectITI.Reprository.ReviewReprositry;
using GraduaitionProjectITI.ViewModel.Review;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;

namespace GraduaitionProjectITI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductReprository reprositryProd;
        private readonly ICategoryReprositry reprositryCaty;
        private readonly IBrandRepository reprositryBrand;
        private readonly IReviewReprositry reprositryReview;
        private readonly UserManager<ApplicationUser> userManager;
        IEnumerable<Product> listnow = new List<Product>();
        private int _brandID=0;

        public HomeController(ILogger<HomeController> logger, IProductReprository reprositryProd,
            ICategoryReprositry reprositryCaty, IBrandRepository reprositryBrand,
            IReviewReprositry reprositryReview, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            this.reprositryProd = reprositryProd;
            this.reprositryCaty = reprositryCaty;
            this.reprositryBrand = reprositryBrand;
            this.userManager = userManager;
            this.reprositryReview = reprositryReview;

        }

        public IActionResult Index()
        {
            _brandID = 0;
            string serializedList = JsonConvert.SerializeObject(_brandID, new JsonSerializerSettings
            { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            byte[] listBytes = Encoding.UTF8.GetBytes(serializedList);
            HttpContext.Session.Set("BrandID", listBytes);
            return View(reprositryProd.GetAll());
        }


        public IActionResult Categories(int catId)
        {
            if (catId != 0)
            {
                byte[] listBytes = HttpContext.Session.Get("BrandID");
                if (listBytes == null)
                {
                    listnow = reprositryProd.GetAll().Where(p => p.CategoryId == catId);
                }
                else
                {
                    string serializedList = Encoding.UTF8.GetString(listBytes);
                    _brandID = JsonConvert.DeserializeObject<int>(serializedList, new JsonSerializerSettings
                    { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

                    if (_brandID == 0)
                    {
                        listnow = reprositryProd.GetAll().Where(p => p.CategoryId == catId);

                    }
                    else
                    {
                        listnow = reprositryProd.GetAll().Where(p => p.CategoryId == catId).Where(p => p.BrandId == _brandID);

                    }


                }

                return PartialView("Products", listnow);
            }


            return PartialView("Products", reprositryProd.GetAll());
        }

        public IActionResult Brands(int BrandId)
        {
            if (BrandId != 0)
            {
                listnow = reprositryProd.GetAll().Where(p => p.BrandId == BrandId);
                _brandID = BrandId;
                string serializedList = JsonConvert.SerializeObject(_brandID, new JsonSerializerSettings
                { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                byte[] listBytes = Encoding.UTF8.GetBytes(serializedList);
                HttpContext.Session.Set("BrandID", listBytes);
                return PartialView("Products", listnow);
            }
            return PartialView("Products", reprositryProd.GetAll());
        }
        [HttpGet]
        public IActionResult GetProduct(int Id)
        {
            var allProducts = reprositryProd.GetAll();
            var currentProduct = allProducts.FirstOrDefault(p => p.Id == Id);
            var randomProducts = allProducts.Except(new List<Product> { currentProduct }).OrderBy(x => Guid.NewGuid()).Take(4);
            var randomProducts2 = allProducts.Except(randomProducts).OrderBy(x => Guid.NewGuid()).Take(4);
            var randomProducts3 = allProducts.Except(randomProducts2).OrderBy(x => Guid.NewGuid()).Take(4);
            var allReviews = reprositryReview.GetReviewsOnSpecificProduct(Id);

            ViewData["Reviews"] = allReviews;
            ViewData["Products"] = randomProducts.ToList();
            ViewData["Products2"] = randomProducts2.ToList();
            ViewData["Products3"] = randomProducts3.ToList();
            var product = reprositryProd.GetProduct(Id);
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> AddReviewAsync(AddReviewViewModel review)
        {

            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(review.email);
                var reviewDB = new AddReviewDBViewModel();
                reviewDB.UserId = user.Id;
                reviewDB.ProdId = review.ProdId;
                reviewDB.Rate = review.Rate;
                reviewDB.Comment = review.Comment;
                reprositryReview.Insert(reviewDB);
                return RedirectToAction("GetProduct", new { Id = review.ProdId });
            }
            else
            {
                ViewData["SignInError"] = "Sign In First!";
                return RedirectToAction("GetProduct", new { Id = review.ProdId });
            }
        }
        public IActionResult Privacy()
        {
            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int? statusCode)
        {



            // Default error view
            return View("NotFound");

        }




        public IActionResult Search(string search) //,int? page)
        {
            if (!string.IsNullOrEmpty(search))
            {
                IEnumerable<Product> listnew = reprositryProd.GetAll().Where(p => p.Name.ToLower().Contains(search.ToLower()));
                return PartialView("Products", listnew); //.ToPagedList(page ?? 1, 1));
            }
            else
            {
                return PartialView("Products", reprositryProd.GetAll());   //.ToPagedList(page ?? 1, 1));
            }
        }
    }
}