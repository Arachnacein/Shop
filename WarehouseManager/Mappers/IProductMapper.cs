using WarehouseManager.Models;
using WarehouseManager.Dtos;

namespace WarehouseManager.Mappers
{
    public interface IProductMapper
    {
        ProductDto Map(Product source);
        Product Map(ProductDto source);
        ICollection<Product> MapElements(ICollection<ProductDto> sources);
        ICollection<ProductDto> MapElements(ICollection<Product> sources);
    }
}