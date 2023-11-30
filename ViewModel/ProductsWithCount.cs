using GraduaitionProjectITI.Models.Entity;

namespace GraduaitionProjectITI.ViewModel
{
    public class ProductsWithCount
    {
        public Product product { get; set; }
        public int Count { get; set; }

        public ProductsWithCount(Product product, int Count)
        {
            this.product = product;
            this.Count = Count;
        }
    }
}
