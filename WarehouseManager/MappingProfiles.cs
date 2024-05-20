using AutoMapper;
using WarehouseManager.Dtos;
using WarehouseManager.Models;

namespace WarehouseManager
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<CreateProductDto, Product>();
            CreateMap<UpdateProductDto, Product>();
            CreateMap<ProductDto, Product>();
            CreateMap<UpdateProductPriceDto, Product>();
            CreateMap<UpdateProductAmountDto, Product>();
            CreateMap<Product, ProductDto>();

        }
    }
}