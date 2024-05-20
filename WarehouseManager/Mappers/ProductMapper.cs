using WarehouseManager.Dtos;
using WarehouseManager.Models;

namespace WarehouseManager.Mappers
{
    public class ProductMapper : IProductMapper
    {
        public ProductDto Map(Product source)
        {
            ProductDto destination = new ProductDto();
            destination.Id = source.Id;
            destination.Name = source.Name;
            destination.Price = source.Price;
            destination.Amount = source.Amount;
            destination.UnitType = source.UnitType;
            destination.ProductType = source.ProductType;
            return destination;
        }
        public Product Map(ProductDto source)
        {
            Product destination = new Product();
            destination.Id = source.Id;
            destination.Name = source.Name;
            destination.Price = source.Price;
            destination.Amount = source.Amount;
            destination.UnitType = source.UnitType;
            destination.ProductType = source.ProductType;
            return destination;
        }
        public ICollection<Product> MapElements(ICollection<ProductDto> sources)
        {
            List<Product> destination = new List<Product>();
            foreach(var item in sources)
                destination.Add(Map(item));
            
            return destination;
        }
        public ICollection<ProductDto> MapElements(ICollection<Product> sources)
        {
            List<ProductDto> destination = new List<ProductDto>();
            foreach(var item in sources)
                destination.Add(Map(item));
            
            return destination;
        }
    }
}