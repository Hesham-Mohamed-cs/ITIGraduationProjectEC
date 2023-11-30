using GraduaitionProjectITI.Models.Context;
using GraduaitionProjectITI.Models.Entity;
using GraduaitionProjectITI.ViewModel;
using GraduaitionProjectITI.ViewModel.Brand_View_Models;
using Microsoft.EntityFrameworkCore;

namespace GraduaitionProjectITI.Reprository.BrandRepository
{
    public class BrandRepository : IBrandRepository
    {
        private readonly ECcontext context;
        private readonly IWebHostEnvironment webHostEnvironment;
        public BrandRepository(ECcontext context, IWebHostEnvironment webHostEnvironment)
        {
            this.context = context;
            this.webHostEnvironment = webHostEnvironment;
        }
        public bool CheckBrandExist(string Name)
        {
            return context.Brands.SingleOrDefault(b => b.Name.ToLower() == Name.ToLower()) == null;
        }

        public bool CheckBrandExistEdit(string Name, int Id)
        {
            return context.Brands.SingleOrDefault(b => b.Id != Id && b.Name.ToLower() == Name.ToLower()) == null;
        }

        public void Delete(int id)
        {
            var brand = GetBrand(id);
            context.Brands.Remove(brand);
            string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "BrandImages");
            string uniqueFileName = brand.Image;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
            Save();
        }
        public IEnumerable<Brand> GetAll()
        {
            return context.Brands.ToList();   
        }

        public void Update(EditBrandViewModel EditBrandViewModel)
        {
            string uniqueFileName = UploadedFile(EditBrandViewModel.BrandImage, EditBrandViewModel.Name);
            var brand = GetBrand(EditBrandViewModel.Id);
            if (uniqueFileName is null)
            {
                // chang name 
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "BrandImages");
                string oldPath = Path.Combine(uploadsFolder, brand.Image);
                var extention = System.IO.Path.GetExtension(oldPath).ToString();
                var imageFullName = EditBrandViewModel.Name + "" + extention;
                string newPath = Path.Combine(uploadsFolder, imageFullName);
                brand.Name = EditBrandViewModel.Name;
                brand.Image = imageFullName;
                File.Move(oldPath, newPath);
            }
            else if (brand.Name != EditBrandViewModel.Name && brand.Image != uniqueFileName && uniqueFileName is not null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "BrandImages");
                var filePath = Path.Combine(uploadsFolder, brand.Image);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                brand.Name = EditBrandViewModel.Name;
                brand.Image = uniqueFileName;

            }
            else
            {
                //chang img 
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "BrandImages");
                var filePath = Path.Combine(uploadsFolder, brand.Image);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    EditBrandViewModel.BrandImage.CopyTo(fileStream);
                }
                brand.Name = EditBrandViewModel.Name;
                brand.Image = uniqueFileName;
            }
        }


        public Brand GetBrand(int id)
        {
            return context.Brands.SingleOrDefault(b => b.Id == id);
        }

        public Brand GetBrandWithProducts(int id)
        {
            return context.Brands.Where(b => b.Id == id).Include(b => b.Products).SingleOrDefault();
        }

        public void Insert(AddBrandViewModel addBrandViewModel)
        {
            string uniqueFileName = UploadedFile(addBrandViewModel.BrandImage, addBrandViewModel.Name);
            var brand = new Brand();
            brand.Name = addBrandViewModel.Name;
            brand.Image = uniqueFileName;
            context.Brands.Add(brand);
            Save();
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public string UploadedFile(IFormFile model, string CatName)
        {
            string uniqueFileName = null;


            if (model != null)
            {

                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "BrandImages");
                uniqueFileName = CatName + System.IO.Path.GetExtension(model.FileName).ToString();

                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                //if (System.IO.File.Exists(filePath))
                //{
                //    System.IO.File.Delete(filePath);
                //}
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
    }
}
