using OrderManager.Dto;
using OrderManager.Models;

namespace OrderManager.Mappers
{
    public class OrderMapper : IOrderMapper
    {
        public Order Map(OrderDto source)
        {
            Order destination = new Order();
            
            destination.Id = source.Id;
            destination.Id_Product = source.Id_Product;
            destination.Id_User = source.Id_User;
            destination.Amount = source.Amount;
            destination.Price = source.Price;
            destination.CreateDate = source.CreateDate;
            destination.CompletionDate = source.CompletionDate;
            destination.Finished = source.Finished;

            return destination;
        }
        public OrderDto Map(Order source)
        {
            OrderDto destination = new OrderDto();
            
            destination.Id = source.Id;
            destination.Id_Product = source.Id_Product;
            destination.Id_User = source.Id_User;
            destination.Amount = source.Amount;
            destination.Price = source.Price;
            destination.CreateDate = source.CreateDate;
            destination.CompletionDate = source.CompletionDate;
            destination.Finished = source.Finished;

            return destination;
        }
        public ICollection<Order> MapElements(ICollection<OrderDto> source)
        {
            List<Order> destination = new List<Order>();
            foreach(var item in source)
                destination.Add(Map(item));

            return destination; 
        }
        public ICollection<OrderDto> MapElements(ICollection<Order> source)
        {
            List<OrderDto> destination = new List<OrderDto>();
            foreach(var item in source)
                destination.Add(Map(item));

            return destination; 
        }
    }
}