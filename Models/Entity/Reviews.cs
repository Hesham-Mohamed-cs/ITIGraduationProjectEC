using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraduaitionProjectITI.Models.Entity
{
    public class Reviews
    {
        public int Id { get; set; }

        [Range(1, 5)]
        [Required]
        public int Rate { get; set; }

        public string Comment { get; set; } = String.Empty;
        public DateTime AddedDateTime { get; set; } = DateTime.Now;

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public  Product Product { get; set; } 
        [ForeignKey("User")]
        public string UserId { get; set; }
        public  ApplicationUser User { get; set; } 
    }
}
