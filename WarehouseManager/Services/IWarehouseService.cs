
using WarehouseManager.Dtos;

namespace WarehouseManager.Services
{
    public interface IWarehouseService
    {
        IEnumerable<ProductDto> GetAllProducts();
        ProductDto GetProduct(int id);
        ProductDto AddNewProduct(CreateProductDto dto);
        void UpdateProduct(UpdateProductDto dto);
        void UpdateProductPrice(UpdateProductPriceDto dto);
        void UpdateProductAmount(UpdateProductAmountDto dto);
        void DeleteProduct(int id);
        int CountProducts();
    }
}