using OrderManager.Dto;
using OrderManager.Models;
using OrderManager.Repositories;
using OrderManager.Data;
using OrderManager.Mappers;
using OrderManager.Exceptions;
using AutoMapper;


namespace OrderManager.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repository;
        private readonly IMapper _mapper;
        private readonly IOrderMapper _orderMapper;

        public OrderService(IOrderRepository repository, IMapper mapper, IOrderMapper orderMapper)
        {
            _repository = repository;
            _mapper = mapper;
            _orderMapper = orderMapper;
        }
        public IEnumerable<OrderDto> GetAllOrders()
        {
            var orders = _repository.GetAll();
            return _orderMapper.MapElements(orders.ToList());
        }
        public OrderDto GetOrderById(int id)
        {
            var order = _repository.GetById(id);
            if(order == null)
                throw new OrderNotFoundException($"Order not found. Id:{id}");
            return _orderMapper.Map(order);
        }
        public OrderDto CreateOrder(CreateOrderDto dto)
        {
            var mappedOrder = _mapper.Map<Order>(dto);

            mappedOrder.CreateDate = DateTime.Now;
            mappedOrder.CompletionDate = DateTime.MinValue;
            mappedOrder.Finished = false;

            _repository.Create(mappedOrder);
            return _orderMapper.Map(mappedOrder);
        }
        public void UpdateOrder(UpdateOrderDto dto)
        {
            var existingOrder = _repository.GetById(dto.Id);
            if(existingOrder == null)
                throw new OrderNotFoundException($"Order not found id:{dto.Id}");
            var mappedOrder = _mapper.Map<Order>(dto);
            mappedOrder.CreateDate = existingOrder.CreateDate;
            _repository.Update(mappedOrder);
        }
        public void FinishOrder(int id)
        {
            var order = _repository.GetById(id);
            if(order == null)
                throw new OrderNotFoundException($"Order not found id:{id}");
            order.Finished = true;
            order.CompletionDate = DateTime.Now;
            _repository.Update(order);
        }
        public void DeleteOrder(int id)
        {
            var existingOrder = _repository.GetById(id);
            if(existingOrder == null)
                throw new OrderNotFoundException($"Order not found id:{id}");
            _repository.Delete(id);
        }
    }
}