using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace GraduaitionProjectITI.ViewModel
{
    public class EditCategoryViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Category Name")]
        [MaxLength(50, ErrorMessage = "category Name Must Be Less Than 50 Char ")]
        [MinLength(3, ErrorMessage = "category Name Must Be Greater Than 3 Char")]
        [Remote(action: "CheckCategoryExistEdit", controller: "Category",AdditionalFields = "Id", ErrorMessage = "Category Name oready Exists ")]
        public string Name { get; set; }

        public  string? Image { get; set; }
        [Display(Name = "category Picture")]
        public IFormFile? CategoryImage { get; set; }
    }
}
