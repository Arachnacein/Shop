using AutoMapper;
using OrderManager.Models;
using OrderManager.Dto;

namespace OrderManager
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<CreateOrderDto, Order>();
            CreateMap<UpdateOrderDto, Order>();
            CreateMap<OrderDto, Order>();
        }
    }
}