using GraduaitionProjectITI.Models.Entity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace GraduaitionProjectITI.ViewModel.Brand_View_Models
{
    public class AddBrandViewModel
    {
        [Display(Name ="Brand Name")]
        [MaxLength(50 ,ErrorMessage = "Brand Name Must Be Less Than 50 Char")]
        [MinLength(3,ErrorMessage ="Brand Name Must be More Than 3 Char")]
        [Remote(action:"CheckBrandExist",controller: "Brand",ErrorMessage = "Brand Already Exists")]
        [Required(ErrorMessage ="Please Enter Brand Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please Choose an Picture")]
        [Display(Name= "Brand Picture")]
        public IFormFile BrandImage { get; set; }

    }
    public class BrandViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IFormFile BrandImage { get; set; }
    }

}
