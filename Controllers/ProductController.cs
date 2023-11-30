using GraduaitionProjectITI.Models.Entity;
using GraduaitionProjectITI.Reprository.BrandRepository;
using GraduaitionProjectITI.Reprository.CategoryReprositry;
using GraduaitionProjectITI.Reprository.ProductReprository;
using GraduaitionProjectITI.ViewModel;
using GraduaitionProjectITI.ViewModel.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml;
using System.Linq;

namespace GraduaitionProjectITI.Controllers
{
    [Authorize(Roles ="Admin")]
    public class ProductController : Controller
    {
        private readonly IProductReprository reprositry;
        private readonly ICategoryReprositry reprositrycaty ;
        private readonly IBrandRepository reprositryBrand;
        public ProductController(IProductReprository reprositry, ICategoryReprositry reprositrycaty , IBrandRepository reprositryBrand)
        {
            this.reprositry = reprositry;
            this.reprositrycaty = reprositrycaty;
            this.reprositryBrand = reprositryBrand;
        }
        public IActionResult Index()
        {
            ViewBag.PageCount = (int)Math.Ceiling((decimal)reprositry.GetAll().Count() / 5m);

            return View(this.reprositry.GetAll());
        }
         public IActionResult GetProducts(int pageNumber, int pageSize = 5)
        {
            var products = reprositry.GetAll()
           .OrderBy(p => p.Id)
           .Skip((pageNumber - 1) * pageSize)
           .Take(pageSize)
           .ToList();
            return PartialView("_ProductTable", products);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(reprositrycaty.GetAll(), "Id", "Name");
            ViewData["BrandId"] = new SelectList(reprositryBrand.GetAll(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(AddProductViewModel addProductViewModel)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    reprositry.Insert(addProductViewModel);
                    reprositry.Save();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    ViewData["CategoryId"] = new SelectList(reprositrycaty.GetAll(), "Id", "Name");
                    ViewData["BrandId"] = new SelectList(reprositryBrand.GetAll(), "Id", "Name");
                    return View(addProductViewModel);
                }
            }
            else
            {
                return View(addProductViewModel);
            }

        }
        
        [HttpGet]
        public IActionResult Edit(int Id)
        {
            var product = reprositry.GetProduct(Id);
            EditProductViewModel editProductViewModel = new EditProductViewModel();

            editProductViewModel.Id = product.Id;
            editProductViewModel.Name = product.Name;
            editProductViewModel.ImgPath = product.ImgPath;
            editProductViewModel.Manufacture = product.Manufacture;
            editProductViewModel.ModelNumber = product.ModelNumber;
            editProductViewModel.Details = product.Details;
            editProductViewModel.Price = product.Price;
            editProductViewModel.Count = product.Count;
            editProductViewModel.CategoryID = product.CategoryId;
            editProductViewModel.BrandID = product.BrandId;
          


            ViewData["CategoryId"] = new SelectList(reprositrycaty.GetAll(), "Id", "Name", reprositrycaty.GetCategory(editProductViewModel.CategoryID));
            ViewData["BrandId"]    = new SelectList(reprositryBrand.GetAll(), "Id", "Name", reprositryBrand.GetBrand(editProductViewModel.BrandID));

            return View(editProductViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EditProductViewModel editProductViewModel)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    reprositry.Update(editProductViewModel);
                    reprositry.Save();
                    return RedirectToAction("Index");

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    ViewData["CategoryId"] = new SelectList(reprositrycaty.GetAll(), "Id", "Name", reprositrycaty.GetCategory(editProductViewModel.CategoryID));
                    ViewData["BrandId"]    = new SelectList(reprositrycaty.GetAll(), "Id", "Name", reprositryBrand.GetBrand(editProductViewModel.CategoryID));
                    return View(editProductViewModel);
                }
            }
            else
            {
                ViewData["CategoryId"] = new SelectList(reprositrycaty.GetAll(), "Id", "Name", reprositrycaty.GetCategory(editProductViewModel.CategoryID));
                ViewData["BrandId"] = new SelectList(reprositrycaty.GetAll(), "Id", "Name", reprositryBrand.GetBrand(editProductViewModel.CategoryID));
                return View(editProductViewModel);
            }
        }

        public IActionResult CheckProductExist(string Name)
        {
            if (reprositry.CheckProductExist(Name))
                return Json(true);
            else
                return Json(false);
        }
        public IActionResult CheckProductExistEdit(string Name, int Id)
        {
            if (reprositry.CheckProductExistEdit(Name, Id))
                return Json(true);
            else
                return Json(false);
        }


        [HttpGet]
        public IActionResult Delete(int Id)
        {
            return View(reprositry.GetProduct(Id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmDelete(int Id)
        {

            if (ModelState.IsValid)
            {
                reprositry.Delete(Id);
                reprositry.Save();
                return RedirectToAction("Index");
            }
            return View("Delete", reprositry.GetProduct(Id));
        }
        [HttpGet]
        public IActionResult GetProduct(int Id)
        {
            var allProducts = reprositry.GetAll();
            var currentProduct = allProducts.FirstOrDefault(p => p.Id == Id);


            var randomProducts = allProducts.Except(new List<Product> { currentProduct }).OrderBy(x => Guid.NewGuid()).Take(4);

            ViewData["Products"] = randomProducts.ToList();
            var product = reprositry.GetProduct(Id);
            return View(product);
        }

        public IActionResult GenerateReport()
        {
            var Products = reprositry.GetAll();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Orders");


                var headerStyle = worksheet.Cells[1, 1, 1, 9].Style;
                headerStyle.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                headerStyle.Fill.BackgroundColor.SetColor(System.Drawing.Color.Black);
                headerStyle.Font.Color.SetColor(System.Drawing.Color.White);

                worksheet.Cells[1, 1].Value = "Product ID";
                worksheet.Cells[1, 2].Value = "Product Name ";
                worksheet.Cells[1, 3].Value = "Manufacture";
                worksheet.Cells[1, 4].Value = "ModelNumber";
                worksheet.Cells[1, 5].Value = "Details";
                worksheet.Cells[1, 6].Value = "Price";
                worksheet.Cells[1, 7].Value = "Count";
                worksheet.Cells[1, 8].Value = "Brand";
                worksheet.Cells[1, 9].Value = "Category";



                worksheet.Cells[worksheet.Dimension.Address].AutoFilter = true;


                worksheet.Column(1).Width = 15;
                worksheet.Column(2).Width = 20;
                worksheet.Column(3).Width = 20;
                worksheet.Column(4).Width = 15;
                worksheet.Column(5).Width = 12;
                worksheet.Column(6).Width = 20;
                worksheet.Column(7).Width = 20;
                worksheet.Column(8).Width = 20;
                worksheet.Column(9).Width = 20;



                int row = 2;
                foreach (var product in Products)
                {
                    worksheet.Cells[row, 1].Value = product.Id.ToString();
                    worksheet.Cells[row, 2].Value = product.Name;
                    worksheet.Cells[row, 3].Value = product.Manufacture;
                    worksheet.Cells[row, 4].Value = product.ModelNumber;
                    worksheet.Cells[row, 5].Value = product.Details;
                    worksheet.Cells[row, 6].Value = product.Price;
                    worksheet.Cells[row, 7].Value = product.Count;
                    worksheet.Cells[row, 8].Value = product.Brand.Name;
                    worksheet.Cells[row, 9].Value = product.Category.Name;


                    row++;
                }
                using (var cells = worksheet.Cells[2, 1, row - 1, 2])
                {
                    cells.Style.Numberformat.Format = "dd/MM/yyyy";
                }
                var excelBytes = package.GetAsByteArray();
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Products Report.xlsx");
            }
        }
    }
}
