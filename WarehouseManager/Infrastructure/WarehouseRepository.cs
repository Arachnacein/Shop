using WarehouseManager.Data;
using WarehouseManager.Models;
using WarehouseManager.Exceptions;

namespace WarehouseManager.Infrastructure
{
    public class WarehouseRepository : IWarehouseRepository
    {
        private readonly WarehouseDbContext _context;
        public WarehouseRepository(WarehouseDbContext context)
        {
            _context = context;
        }
        public bool SaveChanges() => _context.SaveChanges() >= 0;
        public IEnumerable<Product> GetProducts()
        {
            return _context.Products;
        }
        public Product GetProduct(int id)
        {
            return _context.Products.FirstOrDefault(x => x.Id == id);
        }
        public Product AddProduct(Product product)
        {
            if(product == null)
                throw new ArgumentNullException(nameof(product));
            
            _context.Products.Add(product);
            _context.SaveChanges();
            return product;
        }
        public void UpdateProductAmount(Product product, int amountToAdd)
        {
            var productExists = _context.Products.FirstOrDefault(x => x.Id == product.Id);
            if(productExists == null)
                throw new ProductNotFoundException($"Product not found. Id: {product.Id}");
            productExists.Amount += amountToAdd;
            _context.Products.Update(productExists);
            _context.SaveChanges();
        }
        public void UpdateProductPrice(Product product, int newPrice)
        {
            var productExists = _context.Products.FirstOrDefault(x => x.Id == product.Id);
            if(productExists == null)
                throw new ProductNotFoundException($"Product not found. Id: {product.Id}");
            productExists.Price = newPrice;
            _context.Products.Update(productExists);
            _context.SaveChanges();
        }
        public void UpdateProduct(Product product)
        {
            var productExists = _context.Products.FirstOrDefault(x => x.Id == product.Id);
            if(productExists == null)
                throw new ProductNotFoundException($"Product not found. Id: {product.Id}");
            _context.Products.Update(product);
            _context.SaveChanges();
        }
        public void DeleteProduct(int id)
        {
            var product = _context.Products.FirstOrDefault(x => x.Id == id);
            if(product == null)
                throw new ProductNotFoundException($"Product not found. Id: {product.Id}");
            _context.Products.Remove(product);
            _context.SaveChanges();
        }
    }
}