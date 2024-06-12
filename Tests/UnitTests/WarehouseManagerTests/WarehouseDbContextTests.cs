using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Xunit;
using WarehouseManager.Data;
using WarehouseManager.Models;

namespace WarehouseManagerTests
{
    public class WarehouseDbContextTests
    {
        private WarehouseDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<WarehouseDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new WarehouseDbContext(options);
        }

        [Fact]
        public void AddProduct_AddsProductToDatabase()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var product = new Product
            {
                Id = 1,
                Name = "Test Product",
                Price = 10.0M,
                Amount = 100,
                UnitType = UnitEnum.Liter,
                ProductType = ProductTypeEnum.Liquid
            };

            // Act
            dbContext.Products.Add(product);
            dbContext.SaveChanges();

            // Assert
            var savedProduct = dbContext.Products.FirstOrDefault(p => p.Id == product.Id);
            Assert.NotNull(savedProduct);
            Assert.Equal(product.Name, savedProduct.Name);
            Assert.Equal(product.Price, savedProduct.Price);
            Assert.Equal(product.Amount, savedProduct.Amount);
        }

        [Fact]
        public void GetProductById_ReturnsProduct()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var productId = 1;
            var product = new Product
            {
                Id = 1,
                Name = "Test Product",
                Price = 10.0M,
                Amount = 100,
                UnitType = UnitEnum.Liter,
                ProductType = ProductTypeEnum.Liquid
            };
            dbContext.Products.Add(product);
            dbContext.SaveChanges();

            // Act
            var retrievedProduct = dbContext.Products.Find(productId);

            // Assert
            Assert.NotNull(retrievedProduct);
            Assert.Equal(productId, retrievedProduct.Id);
        }

        [Fact]
        public void GetAllProducts_ReturnsAllProducts()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            dbContext.Products.AddRange(
                new Product { Id = 1, Name = "Test Product 1", Price = 10.0M, Amount = 100 },
                new Product { Id = 2, Name = "Test Product 2", Price = 20.0M, Amount = 200 }
            );
            dbContext.SaveChanges();

            // Act
            var products = dbContext.Products.ToList();

            // Assert
            Assert.Equal(2, products.Count);
        }

        [Fact]
        public void UpdateProduct_UpdatesProductInDatabase()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var product = new Product
            {
                Id = 1,
                Name = "Test Product",
                Price = 10.0M,
                Amount = 100,
                UnitType = UnitEnum.Liter,
                ProductType = ProductTypeEnum.Liquid
            };
            dbContext.Products.Add(product);
            dbContext.SaveChanges();

            // Act
            product.Price = 15.0M;
            dbContext.Products.Update(product);
            dbContext.SaveChanges();

            // Assert
            var updatedProduct = dbContext.Products.Find(product.Id);
            Assert.NotNull(updatedProduct);
            Assert.Equal(15.0M, updatedProduct.Price);
        }

        [Fact]
        public void DeleteProduct_DeletesProductFromDatabase()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var product = new Product
            {
                Id = 1,
                Name = "Test Product",
                Price = 10.0M,
                Amount = 100,
                UnitType = UnitEnum.Liter,
                ProductType = ProductTypeEnum.Liquid
            };
            dbContext.Products.Add(product);
            dbContext.SaveChanges();

            // Act
            dbContext.Products.Remove(product);
            dbContext.SaveChanges();

            // Assert
            var deletedProduct = dbContext.Products.Find(product.Id);
            Assert.Null(deletedProduct);
        }
    }
}
