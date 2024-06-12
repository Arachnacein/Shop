using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WarehouseManager.Data;
using WarehouseManager.Exceptions;
using WarehouseManager.Infrastructure;
using WarehouseManager.Models;
using Xunit;

namespace WarehouseManagerTests
{
    public class WarehouseRepositoryTests : IDisposable
    {
        private readonly DbContextOptions<WarehouseDbContext> _options;

        public WarehouseRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<WarehouseDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new WarehouseDbContext(_options))
            {
                context.Products.Add(new Product { Id = 1, Name = "Product1", Price = 10.0m, Amount = 100, UnitType = UnitEnum.Liter, ProductType = ProductTypeEnum.Liquid });
                context.Products.Add(new Product { Id = 2, Name = "Product2", Price = 20.0m, Amount = 200, UnitType = UnitEnum.Kg, ProductType = ProductTypeEnum.Solid });
                context.SaveChanges();
            }
        }

        [Fact]
        public void GetProducts_ReturnsAllProducts()
        {
            // Arrange
            using (var context = new WarehouseDbContext(_options))
            {
                var repository = new WarehouseRepository(context);

                // Act
                var products = repository.GetProducts();

                // Assert
                Assert.Equal(2, products.Count());
            }
        }

        [Fact]
        public void GetProduct_ById_ReturnsProduct()
        {
            // Arrange
            using (var context = new WarehouseDbContext(_options))
            {
                var repository = new WarehouseRepository(context);
                var productId = context.Products.First().Id;

                // Act
                var product = repository.GetProduct(productId);

                // Assert
                Assert.NotNull(product);
                Assert.Equal("Product1", product.Name);
            }
        }

        [Fact]
        public void AddProduct_AddsProduct()
        {
            // Arrange
            using (var context = new WarehouseDbContext(_options))
            {
                var repository = new WarehouseRepository(context);
                var newProduct = new Product { Id = 3, Name = "Product3", Price = 30.0m, Amount = 300, UnitType = UnitEnum.meter, ProductType = ProductTypeEnum.Solid };

                // Act
                var addedProduct = repository.AddProduct(newProduct);

                // Assert
                Assert.NotNull(addedProduct);
                Assert.Equal(3, addedProduct.Id);
                Assert.Equal("Product3", addedProduct.Name);
            }
        }

        [Fact]
        public void UpdateProduct_UpdatesProduct()
        {
            // Arrange
            using (var context = new WarehouseDbContext(_options))
            {
                var repository = new WarehouseRepository(context);
                var productToUpdate = context.Products.First();
                productToUpdate.Name = "UpdatedProduct";

                // Act
                repository.UpdateProduct(productToUpdate);

                // Assert
                Assert.Equal("UpdatedProduct", context.Products.First(p => p.Id == productToUpdate.Id).Name);
            }
        }

        [Fact]
        public void UpdateProductAmount_UpdatesProductAmount()
        {
            // Arrange
            using (var context = new WarehouseDbContext(_options))
            {
                var repository = new WarehouseRepository(context);
                var productToUpdate = context.Products.First();
                var initialAmount = productToUpdate.Amount;

                // Act
                repository.UpdateProductAmount(productToUpdate, 50);

                // Assert
                Assert.Equal(initialAmount + 50, context.Products.First(p => p.Id == productToUpdate.Id).Amount);
            }
        }

        [Fact]
        public void UpdateProductPrice_UpdatesProductPrice()
        {
            // Arrange
            using (var context = new WarehouseDbContext(_options))
            {
                var repository = new WarehouseRepository(context);
                var productToUpdate = context.Products.First();
                var newPrice = 25.0m;

                // Act
                repository.UpdateProductPrice(productToUpdate, (int)newPrice);

                // Assert
                Assert.Equal(newPrice, context.Products.First(p => p.Id == productToUpdate.Id).Price);
            }
        }

        [Fact]
        public void DeleteProduct_DeletesProduct()
        {
            // Arrange
            using (var context = new WarehouseDbContext(_options))
            {
                var repository = new WarehouseRepository(context);
                var productId = context.Products.First().Id;

                // Act
                repository.DeleteProduct(productId);

                // Assert
                Assert.DoesNotContain(context.Products, p => p.Id == productId);
            }
        }

        public void Dispose()
        {
            using (var context = new WarehouseDbContext(_options))
            {
                context.Database.EnsureDeleted();
            }
        }
    }
}
