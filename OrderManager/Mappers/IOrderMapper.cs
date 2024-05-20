using OrderManager.Dto;
using OrderManager.Models;

namespace OrderManager.Mappers
{
    public interface IOrderMapper
    {
        Order Map(OrderDto source);
        OrderDto Map(Order source);
        ICollection<Order> MapElements(ICollection<OrderDto> source);
        ICollection<OrderDto> MapElements(ICollection<Order> source);
    }
}