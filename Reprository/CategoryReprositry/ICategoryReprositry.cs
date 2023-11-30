using GraduaitionProjectITI.Models.Entity;
using GraduaitionProjectITI.ViewModel;

namespace GraduaitionProjectITI.Reprository.CategoryReprositry
{
    public interface ICategoryReprositry
    {
        IEnumerable<Category> GetAll();
        Category GetCategory(int Id);
        Category GetCategoryWithProducts(int Id);
        void Insert(AddCategoryViewModel addCategoryViewModel);
        void Update(EditCategoryViewModel editCategoryViewModel);
        void Delete(int id);
        void Save();
        string UploadedFile(IFormFile model, string CatName);
       bool CheckCategoryExist(string Name);
        bool CheckCategoryExistEdit(string Name, int Id);


    }
}
