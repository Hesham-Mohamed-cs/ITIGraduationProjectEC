using GraduaitionProjectITI.Models.Context;
using GraduaitionProjectITI.Models.Entity;
using GraduaitionProjectITI.ViewModel;
using GraduaitionProjectITI.ViewModel.Products;
using Microsoft.EntityFrameworkCore;

namespace GraduaitionProjectITI.Reprository.ProductReprository
{
    public class ProductReprository : IProductReprository
    {
        private readonly ECcontext context;
        private readonly IWebHostEnvironment webHostEnvironment;
        public ProductReprository(ECcontext context, IWebHostEnvironment webHostEnvironment)
        {
            this.context = context;
            this.webHostEnvironment = webHostEnvironment;
        }

        public bool CheckProductExist(string Name)
        {
            return context.Products.SingleOrDefault(p => p.Name.ToLower() == Name.ToLower()) == null;
        }

        public bool CheckProductExistEdit(string Name, int Id)
        {
            return context.Products.SingleOrDefault(p => p.Id != Id && p.Name.ToLower() == Name.ToLower()) == null;
        }

        public void Delete(int id)
        {
            var product = GetProduct(id);

            context.Products.Remove(product);
            string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "ProductImages");
            string uniqueFileName = product.ImgPath;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
            Save();
        }

        public IEnumerable<Product> GetAll()
        {
            return context.Products.Include(b=>b.Brand).Include(c=>c.Category).Include(c => c.Reviews).ToList();
        }

        public Product GetProduct(int Id)
        {
            return context.Products.SingleOrDefault(c => c.Id == Id);
        }

        public Product GetProductWithOrders(int Id)
        {
            return context.Products.Where(c => c.Id == Id).Include(c => c.subOrders).SingleOrDefault();
        }


        public Product GetProductWithoutReviews(int Id)
        {
            return context.Products.Where(c => c.Id == Id).Include(c => c.Reviews).SingleOrDefault();
        }

        public void Insert(AddProductViewModel addProductViewModel)
        {
            string uniqueFileName = UploadedFile(addProductViewModel.ImgPath, addProductViewModel.Name);
            var product = new Product();

            product.Name = addProductViewModel.Name;
            product.ModelNumber = addProductViewModel.ModelNumber;
            product.Manufacture = addProductViewModel.Manufacture;
            product.Details = addProductViewModel.Details;
            product.Price = addProductViewModel.Price;
            product.CategoryId = addProductViewModel.CategoryID;
            product.Count = addProductViewModel.Count;
            product.BrandId = addProductViewModel.BrandID;
            product.ImgPath = uniqueFileName;
            context.Products.Add(product);
        }
        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(EditProductViewModel editProductViewModel)
        {
            string uniqueFileName = UploadedFile(editProductViewModel.ProductImage, editProductViewModel.Name);
            var product = GetProduct(editProductViewModel.Id);

            if (uniqueFileName is null)
            {
                // chang name 
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "ProductImages");
                string oldPath = Path.Combine(uploadsFolder, product.ImgPath);
                var extention = System.IO.Path.GetExtension(oldPath).ToString();
                var imageFullName = editProductViewModel.Name + "" + extention;
                string newPath = Path.Combine(uploadsFolder, imageFullName);
                product.Name = editProductViewModel.Name;
                product.ImgPath = imageFullName;
                product.ModelNumber = editProductViewModel.ModelNumber;
                product.Manufacture = editProductViewModel.Manufacture;
                product.Details = editProductViewModel.Details;
                product.Price = editProductViewModel.Price;
                product.CategoryId = editProductViewModel.CategoryID;
                product.Count = editProductViewModel.Count;
                product.BrandId = editProductViewModel.BrandID;
                File.Move(oldPath, newPath);
            }
            else if (product.Name != editProductViewModel.Name && product.ImgPath != uniqueFileName && uniqueFileName is not null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "ProductImages");
                var filePath = Path.Combine(uploadsFolder, product.ImgPath);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                product.Name = editProductViewModel.Name;
                product.ImgPath = uniqueFileName;
                product.ModelNumber = editProductViewModel.ModelNumber;
                product.Manufacture = editProductViewModel.Manufacture;
                product.Details = editProductViewModel.Details;
                product.Price = editProductViewModel.Price;
                product.CategoryId = editProductViewModel.CategoryID;
                product.Count = editProductViewModel.Count;
                product.BrandId = editProductViewModel.BrandID;

            }
            else
            {
                //chang img 
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "ProductImages");
                var filePath = Path.Combine(uploadsFolder, product.ImgPath);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    editProductViewModel.ProductImage.CopyTo(fileStream);
                }

                product.Name = editProductViewModel.Name;
                product.ImgPath = uniqueFileName;
                product.ModelNumber = editProductViewModel.ModelNumber;
                product.Manufacture = editProductViewModel.Manufacture;
                product.Details = editProductViewModel.Details;
                product.Price = editProductViewModel.Price;
                product.CategoryId = editProductViewModel.CategoryID;
                product.Count = editProductViewModel.Count;
                product.BrandId = editProductViewModel.BrandID;
            }

        }

        public string UploadedFile(IFormFile model, string ProdName)
        {
            string uniqueFileName = null;


            if (model != null)
            {

                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "ProductImages");
                uniqueFileName = ProdName + System.IO.Path.GetExtension(model.FileName).ToString();

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



        public List<Product> GetProducts(List<int> prodlist)
        {
            var products = new List<Product>();
            foreach (int prod in prodlist)
            {
                GetProduct(prod);
                products.Add(GetProduct(prod));
            }
            return products;
        }
    }
}
