using System.ComponentModel.DataAnnotations;

namespace GraduaitionProjectITI.ViewModel.SubOrders
{
    public class EditSubOrderViewModel
    {
        public int Id {  get; set; } 

        [Display(Name = "SubOrder Amount")]
        [Range(1, 1000, ErrorMessage = "Product Amount Must Be Between 1 and 1000")]
        public int Amount { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
    }
}
