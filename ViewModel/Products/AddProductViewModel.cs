using GraduaitionProjectITI.Models.Entity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace GraduaitionProjectITI.ViewModel.Products
{
    public class AddProductViewModel
    {
        [Display(Name = "Product Name")]
        [MaxLength(50, ErrorMessage = "Product Name Must Be Less Than 50 Char ")]
        [MinLength(3, ErrorMessage = "Product Name Must Be Greater Than 3 Char")]
        [Remote(action: "CheckProductExist", controller: "Product", ErrorMessage = "Product Name oready Exists ")]
        public string Name { get; set; }

        [Display(Name = "Product Manufacture")]
        [MaxLength(50, ErrorMessage = "Product Manufacture Must Be Less Than 50 Char ")]
        [MinLength(3, ErrorMessage = "Product Manufacture Must Be Greater Than 3 Char")]
        public string Manufacture { get; set; }

        [Display(Name = "Product Model Number")]
        [MaxLength(50, ErrorMessage = "Product Model Number Must Be Less Than 50 Char ")]
        [MinLength(3, ErrorMessage = "Product Model Number Must Be Greater Than 3 Char")]
        public string ModelNumber { get; set; }

        [Display(Name = "Product Details")]
        [MaxLength(200, ErrorMessage = "Product Details Must Be Less Than 200 Char ")]
        [MinLength(10, ErrorMessage = "Product Details Must Be Greater Than 10 Char")]
        public string Details { get; set; }

        [Required(ErrorMessage = "Please Choose Product Picture")]
        [Display(Name = "Product Picture")]
        public IFormFile ImgPath { get; set; }

        [Display(Name = "Product Price")]
        [Range(100,100000, ErrorMessage = "Product Price Must Be Between 100 and 100000 Egyption Pound")]
        public double Price { get; set;}

        [Display(Name = "Product Amount")]
        [Range(1, 1000, ErrorMessage = "Product Amount Must Be Between 1 and 1000")]
        public int Count { get; set; }

        [Display(Name = "Product Brand")]
        public int BrandID { get; set; }

        [Display(Name = "Product Category")]
        public int CategoryID { get; set;}


    }
}
