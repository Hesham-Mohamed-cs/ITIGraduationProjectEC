using GraduaitionProjectITI.ViewModel.Cart;

namespace GraduaitionProjectITI.ViewModel.ConfirmOrder
{
    public class UserOrdersDataViewModel
    {
        public List<ProductData> ProductData { get; set; }
        public double TotalCost { get; set; }
        public string UserID { get; set; }
        public string Destination { get; set; }
    }
}
