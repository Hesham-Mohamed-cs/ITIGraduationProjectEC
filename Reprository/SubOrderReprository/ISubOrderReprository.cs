using GraduaitionProjectITI.Models.Entity;
using GraduaitionProjectITI.ViewModel.SubOrders;

namespace GraduaitionProjectITI.Reprository.SubOrderReprository
{
    public interface ISubOrderReprository
    {
        IEnumerable<SubOrder> GetAll();
        SubOrder GetSubOrder(int Id);
        IEnumerable<SubOrder> GetSubOrderForOrder(int Id);
        IEnumerable<SubOrder> GetSubOrderForProduct(int Id);

        void Insert(AddSubOrderViewModel addSubOrderViewModel);
        void Update(EditSubOrderViewModel editSubOrderViewModel);
        void Delete(int id);
        void Save();
    }
}
