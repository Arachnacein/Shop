using WarehouseManager.Models;

namespace WarehouseManager.Data
{
    public interface IWarehouseRepository
     {
        bool SaveChanges();
        IEnumerable<Product> GetProducts();
        Product GetProduct(int id);
        Product AddProduct(Product product);
        void UpdateProductAmount(Product product, int amountToAdd);
        void UpdateProductPrice(Product product, int newPrice);
        void UpdateProduct(Product product);
        void DeleteProduct(int id);
    }
}
