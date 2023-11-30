using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace GraduaitionProjectITI.ViewModel.Brand_View_Models
{
    public class EditBrandViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please Enter Brand Name")]
        [Display(Name = "Brand Name")]
        [MaxLength(50, ErrorMessage = "Brand Name Must Be Less Than 50 Char ")]
        [MinLength(3, ErrorMessage = "Brand Name Must Be Less Than 3 Char")]
        [Remote(action: "CheckBrandExistEdit", controller: "Brand", AdditionalFields = "Id", ErrorMessage = "Brand Name oready Exists ")]
        public string Name { get; set; }

        public string? Image { get; set; }
        [Display(Name = "Brand Picture")]
        public IFormFile? BrandImage { get; set; }
    }
}
