using GraduaitionProjectITI.Reprository.BrandRepository;
using GraduaitionProjectITI.Reprository.CategoryReprositry;
using GraduaitionProjectITI.ViewModel.General;
using Microsoft.AspNetCore.Mvc;

namespace GraduaitionProjectITI.Components.Navbar
{
    public class NavbarViewComponent : ViewComponent
    {
        private readonly IBrandRepository _brandRepository;
        private readonly ICategoryReprositry _categoryReprositry;

        public NavbarViewComponent(IBrandRepository brandRepository,ICategoryReprositry categoryReprositry)
        {
            _brandRepository = brandRepository;
            _categoryReprositry = categoryReprositry;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = _categoryReprositry.GetAll().ToList();
            var brands = _brandRepository.GetAll().ToList();

            var dto = new NavbarDto()
            {
                Categories = categories.Select(x => new ViewModel.CategoryViewModel()
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList(),
                Brands = brands.Select(x => new ViewModel.Brand_View_Models.BrandViewModel()
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList()
            };
            return await Task.FromResult((IViewComponentResult)View("Options", dto));
        }
    }
}
