using System.ComponentModel.DataAnnotations.Schema;

namespace GraduaitionProjectITI.Models.Entity
{
    public class SubOrder
    {
        public int Id { get; set; }
        public int Amount { get; set; }

        [ForeignKey("Order")]
        public int OrderId { get; set;}
        public Order Order { get; set;}
        [ForeignKey("Product")]
        public int ProductId { get; set;}
        public Product Product { get; set;}
    }
}
