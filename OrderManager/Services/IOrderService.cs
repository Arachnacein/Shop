using OrderManager.Dto;

namespace OrderManager.Services
{
    public interface IOrderService
    {
        IEnumerable<OrderDto> GetAllOrders();
        OrderDto GetOrderById(int id);
        OrderDto CreateOrder(CreateOrderDto dto);
        void UpdateOrder(UpdateOrderDto dto);
        void FinishOrder(int id);
        void DeleteOrder(int id);

    }
}