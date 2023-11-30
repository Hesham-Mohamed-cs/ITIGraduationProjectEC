using GraduaitionProjectITI.Models.Context;
using GraduaitionProjectITI.Models.Entity;
using GraduaitionProjectITI.Reprository.CategoryReprositry;
using GraduaitionProjectITI.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GraduaitionProjectITI.Controllers
{
    [Authorize(Roles = "Admin")]

    public class CategoryController : Controller
    {
        private readonly ICategoryReprositry reprositry;
        public CategoryController(ICategoryReprositry reprositry)
        {
            this.reprositry = reprositry;
        }
        public IActionResult Index()
        {
            ViewBag.PageCount = (int)Math.Ceiling((decimal)reprositry.GetAll().Count() / 5m);
            return View(reprositry.GetAll());
        }
        public IActionResult GetCategoriess(int pageNumber, int pageSize = 5)
        {
            var Categories = reprositry.GetAll()
           .OrderBy(p => p.Id)
           .Skip((pageNumber - 1) * pageSize)
           .Take(pageSize)
           .ToList();
            return PartialView("_CategoryTable", Categories);
        }
        [HttpGet]
        public IActionResult Create() 
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(AddCategoryViewModel categoryViewModel)
        {

            if(ModelState.IsValid)
            {

                try
                {
                    reprositry.Insert(categoryViewModel);
                    reprositry.Save();
                    return RedirectToAction("Index");

                }
                catch ( Exception ex )
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(categoryViewModel);

                }
            }
            else
            {
                return View(categoryViewModel);
            }

        }
        public IActionResult CheckCategoryExist(string Name)
        {
            if (reprositry.CheckCategoryExist(Name))
                return Json(true);
            else
                return Json(false);
        }
        //private string UploadedFile(IFormFile model , string CatName)
        //{
        //    string uniqueFileName = null;


        //    if (model != null)
        //    {
               
        //        string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "CategoryImages");
        //        //uniqueFileName = Guid.NewGuid().ToString() + "_" + model.FileName;
        //        uniqueFileName = CatName + System.IO.Path.GetExtension(model.FileName).ToString();

        //        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
        //        if (System.IO.File.Exists(filePath))
        //        {
        //            System.IO.File.Delete(filePath);
        //        }
        //        using (var fileStream = new FileStream(filePath, FileMode.Create))
        //        {
        //            model.CopyTo(fileStream);
        //        }
        //    }
        //    return uniqueFileName;
        //}

        [HttpGet]
        public IActionResult Edit(int Id)
        {
           var category =  reprositry.GetCategory( Id) ;
            EditCategoryViewModel categoryViewModel = new EditCategoryViewModel();
            categoryViewModel.Id = category.Id;
            categoryViewModel.Name = category.Name;
            categoryViewModel.Image = category.Image;
            return View(categoryViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EditCategoryViewModel categoryViewModel)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    reprositry.Update(categoryViewModel);
                    reprositry.Save();
                    return RedirectToAction("Index");

                }
                catch (Exception ex )
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(categoryViewModel);
                }
            }
            else
            {
                return View(categoryViewModel);
            }
        }
        public IActionResult CheckCategoryExistEdit(string Name,int Id)
        {
            if (reprositry.CheckCategoryExistEdit(Name , Id))
                return Json(true);
            else
                return Json(false);
        }


        [HttpGet]
        public IActionResult Delete(int Id)
        {
            var data = reprositry.GetCategoryWithProducts(Id);
            ViewBag.flag = data.Products.Count > 0? true : false;
            return View(data);
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
            return View("Delete");


        }
    }
}

