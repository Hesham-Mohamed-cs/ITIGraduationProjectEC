using GraduaitionProjectITI.Models.Context;
using GraduaitionProjectITI.Models.Entity;
using GraduaitionProjectITI.ViewModel;
using GraduaitionProjectITI.ViewModel.SubOrders;
using Microsoft.EntityFrameworkCore;

namespace GraduaitionProjectITI.Reprository.SubOrderReprository
{
    public class SubOrderReprository : ISubOrderReprository
    {
        private readonly ECcontext context;

        public SubOrderReprository(ECcontext context)
        {
            this.context = context;
        }

        public void Delete(int id)
        {
            var SubOrder = GetSubOrder(id);
            context.subOrders.Remove(SubOrder);
            
        }

        public IEnumerable<SubOrder> GetAll()
        {
            return context.subOrders.Include(c=>c.Product).ToList();

        }

        public SubOrder GetSubOrder(int Id)
        {
            return context.subOrders.SingleOrDefault(c => c.Id == Id);

        }

        public IEnumerable<SubOrder> GetSubOrderForOrder(int Id)
        {
            return context.subOrders.Where(c => c.OrderId == Id).Include(c => c.Product).ToList();
        }

        public IEnumerable<SubOrder> GetSubOrderForProduct(int Id)
        {
            return context.subOrders.Where(c => c.ProductId == Id).Include(c => c.Product).ToList();

        }
        public void Insert(AddSubOrderViewModel addSubOrderViewModel)
        {
            var Suborder = new SubOrder();
            Suborder.Amount = addSubOrderViewModel.Amount;
            Suborder.ProductId = addSubOrderViewModel.ProductId;
            Suborder.OrderId = addSubOrderViewModel.OrderId;
            context.subOrders.Add(Suborder);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(EditSubOrderViewModel editSubOrderViewModel)
        {
            var Suborder = GetSubOrder(editSubOrderViewModel.Id);
            Suborder.Amount = editSubOrderViewModel.Amount;
            Suborder.ProductId = editSubOrderViewModel.ProductId;
            Suborder.OrderId = editSubOrderViewModel.OrderId;
        }

        
    }
}
