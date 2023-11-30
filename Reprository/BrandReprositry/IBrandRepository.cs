using GraduaitionProjectITI.Models.Entity;
using GraduaitionProjectITI.ViewModel.Brand_View_Models;

namespace GraduaitionProjectITI.Reprository.BrandRepository
{
    public interface IBrandRepository
    {
        IEnumerable<Brand> GetAll();
        Brand GetBrand(int id);
        Brand GetBrandWithProducts(int id);
        void Insert(AddBrandViewModel addBrandViewModel);
        void Update(EditBrandViewModel EditBrandViewModel);
        void Delete(int id);
        void Save();
        string UploadedFile(IFormFile model, string CatName);
        bool CheckBrandExist(string Name);
        bool CheckBrandExistEdit(string Name, int Id);

    }
}
