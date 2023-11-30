using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraduaitionProjectITI.Models.Entity
{
    public class Product
    {
        public int Id { get; set; }
        [MaxLength(100, ErrorMessage = "Product Name Must Be Less Than 100 char  ")]
        public string Name { get; set; } = String.Empty;
        [MaxLength(100, ErrorMessage = "Product Manufacture Must Be Less Than 100 char  ")]
        public string Manufacture { get; set; } = String.Empty;

        [MaxLength(100, ErrorMessage = "Product Model Number Must Be Less Than 100 char  ")]
        public string ModelNumber { get; set; } = String.Empty;
       
        [MaxLength(200, ErrorMessage = "Product Description Must Be Less Than 200 char  ")]
        public string Details { get; set; } = String.Empty;
        public string ImgPath { get; set; } = String.Empty;

        [Range(1, 10000)]
        public double Price { get; set; } 
        [Range(minimum:0, maximum:1000)]
        public int Count { get; set; }

        [ForeignKey("Brand")]
        public int BrandId { get; set; }
        [ForeignKey("Category")]

        public int CategoryId { get; set; }
        public Brand Brand { get; set; } 
        public Category Category { get; set; }
        public ICollection<SubOrder> subOrders { get; set; }
        public ICollection<Reviews> Reviews { get; set; }

    }
}
