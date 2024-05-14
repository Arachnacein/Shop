using OrderManager.Models;
using OrderManager.Data;
using OrderManager.Exceptions;

namespace OrderManager.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderDbContext _context;
        public OrderRepository(OrderDbContext context)
        {
            _context = context;
        }

        public bool SaveChanges() =>  _context.SaveChanges() >= 0;
        
        public IEnumerable<Order> GetAll()
        {
            return _context.Orders;
        }
        public Order GetById(int id)
        {
            return _context.Orders.FirstOrDefault(x => x.Id == id);
        }
        public Order Create(Order order)
        {
            if(order == null)
                throw new ArgumentNullException(nameof(order));
            _context.Orders.Add(order);
            _context.SaveChanges();
            return order;
        }
        public void Update(Order order)
        {
            var existingOrder = _context.Orders.FirstOrDefault(x => x.Id == order.Id);
            if(existingOrder == null)
                throw new OrderNotFoundException($"Order not found. Id: {order.Id}");
            _context.Orders.Update(order);
            _context.SaveChanges();
        }
        public void Delete(int id)
        {
            var existingOrder = _context.Orders.FirstOrDefault(x => x.Id == id);
            if(existingOrder == null)
                throw new OrderNotFoundException($"Order not found. Id: {id}");
            
            _context.Orders.Remove(existingOrder);
            _context.SaveChanges();

        }

        public IEnumerable<Order> GetAllFinished() => _context.Orders.Where(x => x.Finished == true);
        public IEnumerable<Order> GetAllUnfinished()=> _context.Orders.Where(x => x.Finished == false);
        public IEnumerable<Order> GetAllFinished(Guid userId) =>_context.Orders.Where(x => x.Finished == true && x.Id_User == userId);
        public IEnumerable<Order> GetAllUnfinished(Guid userId) =>_context.Orders.Where(x => x.Finished == false && x.Id_User == userId);
    }

}