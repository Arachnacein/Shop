using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Xunit;
using AutoMapper;
using WarehouseManager.Data;
using WarehouseManager.Exceptions;
using WarehouseManager.Models;
using WarehouseManager.Dtos;
using WarehouseManager.Mappers;
using WarehouseManager.Services;

namespace WarehouseManagerTests
{
    public class WarehouseServiceTests
    {
        private readonly Mock<IWarehouseRepository> _mockWarehouseRepository;
        private readonly Mock<IProductMapper> _mockProductMapper;
        private readonly Mock<IMapper> _mockMapper;
        private readonly WarehouseService _warehouseService;

        public WarehouseServiceTests()
        {
            _mockWarehouseRepository = new Mock<IWarehouseRepository>();
            _mockProductMapper = new Mock<IProductMapper>();
            _mockMapper = new Mock<IMapper>();
            _warehouseService = new WarehouseService(_mockWarehouseRepository.Object, _mockProductMapper.Object, _mockMapper.Object);
        }

        [Fact]
        public void GetAllProducts_WhenProductsExist_ReturnsProductDtos()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Product 1", Price = 10.5m, Amount = 100, UnitType = UnitEnum.Liter, ProductType = ProductTypeEnum.Liquid },
                new Product { Id = 2, Name = "Product 2", Price = 20.75m, Amount = 50, UnitType = UnitEnum.Kg, ProductType = ProductTypeEnum.Solid }
            };
            _mockWarehouseRepository.Setup(repo => repo.GetProducts()).Returns(products);
            _mockProductMapper.Setup(mapper => mapper.MapElements(products)).Returns(new List<ProductDto>());

            // Act
            var result = _warehouseService.GetAllProducts();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result); // Assuming mapper returns an empty list
        }

        [Fact]
        public void GetAllProducts_WhenNoProductsExist_ThrowsArgumentNullException()
        {
            // Arrange
            _mockWarehouseRepository.Setup(repo => repo.GetProducts()).Returns(new List<Product>());

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _warehouseService.GetAllProducts());
        }

        [Fact]
        public void GetProduct_WhenProductExists_ReturnsProductDto()
        {
            // Arrange
            var productId = 1;
            var product = new Product { Id = productId, Name = "Product 1", Price = 10.5m, Amount = 100, UnitType = UnitEnum.Liter, ProductType = ProductTypeEnum.Liquid };
            var productDto = new ProductDto { Id = productId, Name = "Product 1", Price = 10.5m, Amount = 100, UnitType = UnitEnum.Liter, ProductType = ProductTypeEnum.Liquid };
            _mockWarehouseRepository.Setup(repo => repo.GetProduct(productId)).Returns(product);
            _mockMapper.Setup(mapper => mapper.Map<ProductDto>(product)).Returns(productDto);

            // Act
            var result = _warehouseService.GetProduct(productId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(productDto, result);
        }

        [Fact]
        public void GetProduct_WhenProductDoesNotExist_ThrowsProductNotFoundException()
        {
            // Arrange
            var productId = 1;
            _mockWarehouseRepository.Setup(repo => repo.GetProduct(productId)).Returns((Product)null);

            // Act & Assert
            var exception = Assert.Throws<ProductNotFoundException>(() => _warehouseService.GetProduct(productId));
            Assert.Equal($"Product not found! ID:{productId}", exception.Message);
        }

        [Fact]
        public void AddNewProduct_ValidProduct_ReturnsProductDto()
        {
            // Arrange
            var createProductDto = new CreateProductDto { Name = "New Product", Price = 15.5m, Amount = 50, UnitType = "Litr", ProductType = "Ciekły" };
            var productDto = new ProductDto { Id = 1, Name = "New Product", Price = 15.5m, Amount = 50, UnitType = UnitEnum.Liter, ProductType = ProductTypeEnum.Liquid };

            _mockWarehouseRepository.Setup(repo => repo.AddProduct(It.IsAny<Product>())).Callback((Product p) =>
            {
                p.Id = 1; // Ustawienie Id na 1 dla potwierdzenia dodania produktu
            });
            _mockProductMapper.Setup(mapper => mapper.Map(It.IsAny<Product>())).Returns(productDto);

            var warehouseService = new WarehouseService(_mockWarehouseRepository.Object, _mockProductMapper.Object, _mockMapper.Object);

            // Act
            var result = warehouseService.AddNewProduct(createProductDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(productDto, result);

        }


        [Fact]
        public void UpdateProduct_WhenProductExists_UpdatesProduct()
        {
            // Arrange
            var updateProductDto = new UpdateProductDto { Id = 1, Name = "Updated Product", Price = 20.5m, Amount = 75, UnitType = "Kilogram", ProductType = "Sypki" };
            var existingProduct = new Product { Id = 1, Name = "Existing Product", Price = 15.5m, Amount = 50, UnitType = UnitEnum.Liter, ProductType = ProductTypeEnum.Liquid };
            _mockWarehouseRepository.Setup(repo => repo.GetProduct(updateProductDto.Id)).Returns(existingProduct);

            // Act
            _warehouseService.UpdateProduct(updateProductDto);

            // Assert
            _mockWarehouseRepository.Verify(repo => repo.UpdateProduct(It.IsAny<Product>()), Times.Once);
        }

        [Fact]
        public void UpdateProduct_WhenProductDoesNotExist_ThrowsProductNotFoundException()
        {
            // Arrange
            var updateProductDto = new UpdateProductDto { Id = 1, Name = "Updated Product", Price = 20.5m, Amount = 75, UnitType = "Kilogram", ProductType = "Sypki" };
            _mockWarehouseRepository.Setup(repo => repo.GetProduct(updateProductDto.Id)).Returns((Product)null);

            // Act & Assert
            var exception = Assert.Throws<ProductNotFoundException>(() => _warehouseService.UpdateProduct(updateProductDto));
            Assert.Equal($"Product not found! ID:{updateProductDto.Id}", exception.Message);
        }

        [Fact]
        public void UpdateProductPrice_WhenProductExists_UpdatesProductPrice()
        {
            // Arrange
            var updateProductPriceDto = new UpdateProductPriceDto { Id = 1, Price = 25.5m };
            var existingProduct = new Product { Id = 1, Name = "Existing Product", Price = 15.5m, Amount = 50, UnitType = UnitEnum.Liter, ProductType = ProductTypeEnum.Liquid };
            _mockWarehouseRepository.Setup(repo => repo.GetProduct(updateProductPriceDto.Id)).Returns(existingProduct);

            // Act
            _warehouseService.UpdateProductPrice(updateProductPriceDto);

            // Assert
            _mockWarehouseRepository.Verify(repo => repo.UpdateProduct(It.IsAny<Product>()), Times.Once);
        }

        [Fact]
        public void UpdateProductPrice_WhenProductDoesNotExist_ThrowsProductNotFoundException()
        {
            // Arrange
            var updateProductPriceDto = new UpdateProductPriceDto { Id = 1, Price = 25.5m };
            _mockWarehouseRepository.Setup(repo => repo.GetProduct(updateProductPriceDto.Id)).Returns((Product)null);

            // Act & Assert
            var exception = Assert.Throws<ProductNotFoundException>(() => _warehouseService.UpdateProductPrice(updateProductPriceDto));
            Assert.Equal($"Product not found! ID:{updateProductPriceDto.Id}", exception.Message);
        }

        [Fact]
        public void UpdateProductPrice_WhenPriceIsLessThanZero_ThrowsBadValueException()
        {
            // Arrange
            var updateProductPriceDto = new UpdateProductPriceDto { Id = 1, Price = -5.0m };
            var existingProduct = new Product { Id = 1, Name = "Existing Product", Price = 15.5m, Amount = 50, UnitType = UnitEnum.Liter, ProductType = ProductTypeEnum.Liquid };
            _mockWarehouseRepository.Setup(repo => repo.GetProduct(updateProductPriceDto.Id)).Returns(existingProduct);

            // Act & Assert
            var exception = Assert.Throws<BadValueException>(() => _warehouseService.UpdateProductPrice(updateProductPriceDto));
            Assert.Equal($"Parameter price has incorrect value = {updateProductPriceDto.Price} should be greater than zero.", exception.Message);
        }

        [Fact]
        public void UpdateProductAmount_WhenProductExists_UpdatesProductAmount()
        {
            // Arrange
            var updateProductAmountDto = new UpdateProductAmountDto { Id = 1, Amount = 25 };
            var existingProduct = new Product { Id = 1, Name = "Existing Product", Price = 15.5m, Amount = 50, UnitType = UnitEnum.Liter, ProductType = ProductTypeEnum.Liquid };
            _mockWarehouseRepository.Setup(repo => repo.GetProduct(updateProductAmountDto.Id)).Returns(existingProduct);

            // Act
            _warehouseService.UpdateProductAmount(updateProductAmountDto);

            // Assert
            _mockWarehouseRepository.Verify(repo => repo.UpdateProduct(It.IsAny<Product>()), Times.Once);
        }

        [Fact]
        public void UpdateProductAmount_WhenProductDoesNotExist_ThrowsProductNotFoundException()
        {
            // Arrange
            var updateProductAmountDto = new UpdateProductAmountDto { Id = 1, Amount = 25 };
            _mockWarehouseRepository.Setup(repo => repo.GetProduct(updateProductAmountDto.Id)).Returns((Product)null);

            // Act & Assert
            var exception = Assert.Throws<ProductNotFoundException>(() => _warehouseService.UpdateProductAmount(updateProductAmountDto));
            Assert.Equal($"Product not found! ID:{updateProductAmountDto.Id}", exception.Message);
        }

        [Fact]
        public void DeleteProduct_WhenProductExists_DeletesProduct()
        {
            // Arrange
            var productId = 1;
            var existingProduct = new Product { Id = productId, Name = "Existing Product", Price = 15.5m, Amount = 50, UnitType = UnitEnum.Liter, ProductType = ProductTypeEnum.Liquid };
            _mockWarehouseRepository.Setup(repo => repo.GetProduct(productId)).Returns(existingProduct);

            // Act
            _warehouseService.DeleteProduct(productId);

            // Assert
            _mockWarehouseRepository.Verify(repo => repo.DeleteProduct(productId), Times.Once);
        }

        [Fact]
        public void DeleteProduct_WhenProductDoesNotExist_ThrowsProductNotFoundException()
        {
            // Arrange
            var productId = 1;
            _mockWarehouseRepository.Setup(repo => repo.GetProduct(productId)).Returns((Product)null);

            // Act & Assert
            var exception = Assert.Throws<ProductNotFoundException>(() => _warehouseService.DeleteProduct(productId));
            Assert.Equal($"Product not found! ID:{productId}", exception.Message);
        }

        [Fact]
        public void CountProducts_ReturnsNumberOfProducts()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Product 1", Price = 10.5m, Amount = 100, UnitType = UnitEnum.Liter, ProductType = ProductTypeEnum.Liquid },
                new Product { Id = 2, Name = "Product 2", Price = 20.75m, Amount = 50, UnitType = UnitEnum.Kg, ProductType = ProductTypeEnum.Solid }
            };
            _mockWarehouseRepository.Setup(repo => repo.GetProducts()).Returns(products);

            // Act
            var result = _warehouseService.CountProducts();

            // Assert
            Assert.Equal(products.Count, result);
        }

        [Fact]
        public void Check_WhenProductExistsAndAmountIsSufficient_ReturnsTrue()
        {
            // Arrange
            var productId = 1;
            var productAmount = 20;
            var product = new Product { Id = productId, Name = "Product 1", Price = 10.5m, Amount = 100, UnitType = UnitEnum.Liter, ProductType = ProductTypeEnum.Liquid };
            _mockWarehouseRepository.Setup(repo => repo.GetProduct(productId)).Returns(product);

            // Act
            var result = _warehouseService.Check(productId, productAmount);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Check_WhenProductExistsAndAmountIsInsufficient_ReturnsFalse()
        {
            // Arrange
            var productId = 1;
            var productAmount = 120;
            var product = new Product { Id = productId, Name = "Product 1", Price = 10.5m, Amount = 100, UnitType = UnitEnum.Liter, ProductType = ProductTypeEnum.Liquid };
            _mockWarehouseRepository.Setup(repo => repo.GetProduct(productId)).Returns(product);

            // Act
            var result = _warehouseService.Check(productId, productAmount);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Check_WhenProductDoesNotExist_ThrowsProductNotFoundException()
        {
            // Arrange
            var productId = 1;
            _mockWarehouseRepository.Setup(repo => repo.GetProduct(productId)).Returns((Product)null);

            // Act & Assert
            var exception = Assert.Throws<ProductNotFoundException>(() => _warehouseService.Check(productId, 20));
            Assert.Equal($"Product not found id:{productId}", exception.Message);
        }

        [Fact]
        public void GetProductPrice_WhenProductExists_ReturnsProductPrice()
        {
            // Arrange
            var productId = 1;
            var productPrice = 10.5m;
            var product = new Product { Id = productId, Name = "Product 1", Price = productPrice, Amount = 100, UnitType = UnitEnum.Liter, ProductType = ProductTypeEnum.Liquid };
            _mockWarehouseRepository.Setup(repo => repo.GetProduct(productId)).Returns(product);

            // Act
            var result = _warehouseService.GetProductPrice(productId);

            // Assert
            Assert.Equal(productPrice, result);
        }

        [Fact]
        public void GetProductPrice_WhenProductDoesNotExist_ThrowsProductNotFoundException()
        {
            // Arrange
            var productId = 1;
            _mockWarehouseRepository.Setup(repo => repo.GetProduct(productId)).Returns((Product)null);

            // Act & Assert
            var exception = Assert.Throws<ProductNotFoundException>(() => _warehouseService.GetProductPrice(productId));
            Assert.Equal($"Product not found id:{productId}", exception.Message);
        }
    }
}

