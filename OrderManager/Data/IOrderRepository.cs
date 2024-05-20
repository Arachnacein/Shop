using OrderManager.Models;

namespace OrderManager.Data
{
    public interface IOrderRepository
    {
        bool SaveChanges();
        IEnumerable<Order> GetAll();
        Order GetById(int id);
        Order Create(Order order);
        void Update(Order order);
        void Delete(int id);

        IEnumerable<Order> GetAllFinished();
        IEnumerable<Order> GetAllUnfinished();
        IEnumerable<Order> GetAllFinished(Guid userId);
        IEnumerable<Order> GetAllUnfinished(Guid userId);
    }
}
