using GraduaitionProjectITI.Models.Context;
using GraduaitionProjectITI.Reprository.BrandRepository;
using GraduaitionProjectITI.ViewModel;
using GraduaitionProjectITI.ViewModel.Brand_View_Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;

namespace GraduaitionProjectITI.Controllers
{
    [Authorize(Roles = "Admin")]

    public class BrandController : Controller
    {
        
        private readonly IBrandRepository brandRepository;
        public BrandController(IBrandRepository brandRepository)
        {
            this.brandRepository = brandRepository;
        }
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.PageCount = (int)Math.Ceiling((decimal)brandRepository.GetAll().Count() / 5m);

            return View(brandRepository.GetAll());
        }
        
        public IActionResult GetBrands(int pageNumber, int pageSize = 5)
        {
            var Brands = brandRepository.GetAll()
           .OrderBy(p => p.Id)
           .Skip((pageNumber - 1) * pageSize)
           .Take(pageSize)
           .ToList();
            return PartialView("_BrandTable", Brands);
        }
        [HttpGet]
        public IActionResult Create() 
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(AddBrandViewModel addBrandViewModel)
        {
            if(ModelState.IsValid) 
            {
                try
                {
                    brandRepository.Insert(addBrandViewModel);
                    brandRepository.Save();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("",errorMessage: ex.Message);
                    return View(addBrandViewModel);
                }
            }
            else
            {
                return View(addBrandViewModel);
            }

        }
        public IActionResult CheckBrandExist(string Name)
        {
            if (brandRepository.CheckBrandExist(Name))
                return Json(true);
            else
                return Json(false);
        }
        [HttpGet]
        public IActionResult Edit(int Id)
        {
            var brand = brandRepository.GetBrand(Id);
            EditBrandViewModel brandViewModel = new EditBrandViewModel();
            brandViewModel.Id = brand.Id;
            brandViewModel.Name = brand.Name;
            brandViewModel.Image = brand.Image;
            return View(brandViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EditBrandViewModel brandViewModel)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    brandRepository.Update(brandViewModel);
                    brandRepository.Save();
                    return RedirectToAction("Index");

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(brandViewModel);
                }
            }
            else
            {
                return View(brandViewModel);
            }
        }
        public IActionResult CheckBrandExistEdit(string Name, int Id)
        {
            if (brandRepository.CheckBrandExistEdit(Name, Id))
                return Json(true);
            else
                return Json(false);
        }
        [HttpGet]
        public IActionResult Delete(int Id)
        {
            var data = brandRepository.GetBrandWithProducts(Id);
            ViewBag.flag = data.Products.Count > 0 ? true : false;
            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmDelete(int Id)
        {

            if (ModelState.IsValid)
            {
                brandRepository.Delete(Id);
                return RedirectToAction("Index");
            }
            return View("Delete");
        }
    }
}
