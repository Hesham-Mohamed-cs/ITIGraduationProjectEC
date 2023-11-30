using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GraduaitionProjectITI.Models.Entity
{
    public class Order
    {
        public int Id { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime ResevationDate { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime DeliveryDate { get; set; }
        public double Cost { get; set; }
        [MaxLength(100 , ErrorMessage ="Destination Must Be Less Than 100 char ")]
        public string destination { get; set; }

        public bool IsConfirmed { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        public  ApplicationUser User { get; set; } 

        public ICollection<SubOrder> subOrders { get; set; }

    }
}
