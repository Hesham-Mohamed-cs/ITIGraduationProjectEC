using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace GraduaitionProjectITI.ViewModel
{
    public class AddCategoryViewModel
    {
        [Display(Name = "Category Name")]
        [MaxLength(50, ErrorMessage = "category Name Must Be Less Than 50 Char ")]
        [MinLength(3,ErrorMessage = "category Name Must Be Greater Than 3 Char")]
        [Remote(action:"CheckCategoryExist" , controller:"Category" , ErrorMessage ="Category Name oready Exists ")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please choose Category Picture ")]
        [Display(Name = "category Picture")]
        public IFormFile CategoryImage { get; set; }
    }

    public class CategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IFormFile CategoryImage { get; set; }
    }

}
