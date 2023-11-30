using GraduaitionProjectITI.Models.Entity;
using GraduaitionProjectITI.ViewModel;

namespace GraduaitionProjectITI.Reprository.OrderReprository
{
    public interface IOrderReprository
    {
        IEnumerable<Order> GetAll();
        Order GetOrder(int Id);
        Order GetOrderWithSubOrders(int Id);
        void Insert(AddOrderViewModel addOrderViewModel);
        void Update(EditOrderViewModel editOrderViewModel);
        void Delete(int id);
        void Save();
        Order InsertNew(AddOrderViewModel addOrderViewModel);
    }
}
