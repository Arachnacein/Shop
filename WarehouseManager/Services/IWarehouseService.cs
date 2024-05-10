
using WarehouseManager.Dtos;

namespace WarehouseManager.Services
{
    public interface IWarehouseService
    {
        IEnumerable<ProductDto> GetAllProducts();
        ProductDto GetProduct(int id);
        ProductDto AddNewProduct(CreateProductDto dto);
        void UpdateProduct(UpdateProductDto dto);
        void UpdateProductPrice(UpdateProductPriceDto dto, decimal price);
        void UpdateProductAmount(UpdateProductAmountDto dto, int amount);
        void DeleteProduct(int id);
        int CountProducts();
    }
}