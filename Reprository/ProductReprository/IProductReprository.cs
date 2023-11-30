using GraduaitionProjectITI.Models.Entity;
using GraduaitionProjectITI.ViewModel;
using GraduaitionProjectITI.ViewModel.Products;

namespace GraduaitionProjectITI.Reprository.ProductReprository
{
    public interface IProductReprository
    {
        IEnumerable<Product> GetAll();
        Product GetProduct(int Id);
        Product GetProductWithOrders(int Id);
        Product GetProductWithoutReviews(int Id);
        void Insert(AddProductViewModel addProductViewModel);
        void Update(EditProductViewModel editProductViewModel);
        void Delete(int id);
        void Save();
        string UploadedFile(IFormFile model, string ProdName);
        bool CheckProductExist(string Name);
        bool CheckProductExistEdit(string Name, int Id);

        List<Product> GetProducts(List<int>? _productsSelected);



    }
}
