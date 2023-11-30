using System.ComponentModel.DataAnnotations;

namespace GraduaitionProjectITI.Models.Entity
{
    public class Category
    {
        public int Id { get; set; }
        [MaxLength(100, ErrorMessage = "category Name Must Be Less Than 100 Char ")]
        public string Name { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;

        public ICollection<Product> Products { get; set; } 
    }
}
