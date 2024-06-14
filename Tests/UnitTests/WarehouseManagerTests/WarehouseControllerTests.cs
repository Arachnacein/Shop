using Microsoft.AspNetCore.Mvc;
using Moq;
using WarehouseManager.Controllers;
using WarehouseManager.Dtos;
using WarehouseManager.Exceptions;
using WarehouseManager.Services;
using Xunit;

namespace WarehouseManagerTests
{
    public class ProductControllerTests
    {
        private readonly Mock<IWarehouseService> _mockService;
        private readonly ProductController _controller;

        public ProductControllerTests()
        {
            _mockService = new Mock<IWarehouseService>();
            _controller = new ProductController(_mockService.Object);
        }

        [Fact]
        public void Get_ReturnsOkObjectResult()
        {
            _mockService.Setup(service => service.GetAllProducts()).Returns(new[] { new ProductDto() });

            var result = _controller.Get();

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void Get_ReturnsNotFoundResult_WhenProductNotFoundException()
        {
            _mockService.Setup(service => service.GetAllProducts()).Throws(new ProductNotFoundException("Product not found!"));

            var result = _controller.Get();

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Product not found!", notFoundResult.Value);
        }

        [Fact]
        public void GetById_ReturnsOkObjectResult()
        {
            _mockService.Setup(service => service.GetProduct(It.IsAny<int>())).Returns(new ProductDto());

            var result = _controller.GetById(1);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void GetById_ReturnsNotFoundResult_WhenProductNotFoundException()
        {
            _mockService.Setup(service => service.GetProduct(It.IsAny<int>())).Throws(new ProductNotFoundException("Product not found!"));

            var result = _controller.GetById(1);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Product not found!", notFoundResult.Value);
        }

        [Fact]
        public void GetPrice_ReturnsOkObjectResult()
        {
            _mockService.Setup(service => service.GetProductPrice(It.IsAny<int>())).Returns(10.0M);

            var result = _controller.GetPrice(1);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void GetPrice_ReturnsNotFoundResult_WhenProductNotFoundException()
        {
            int productId = 1;
            _mockService.Setup(service => service.GetProductPrice(productId)).Throws(new ProductNotFoundException("Product not found!"));

            var result = _controller.GetPrice(productId);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Product not found!", notFoundResult.Value);
        }

        [Fact]
        public void Create_ReturnsCreatedResult()
        {
            var dto = new CreateProductDto();

            _mockService.Setup(service => service.AddNewProduct(It.IsAny<CreateProductDto>())).Returns(new ProductDto());

            var result = _controller.Create(dto);

            Assert.IsType<CreatedResult>(result);
        }

        [Fact]
        public void Create_ReturnsConflictResult_WhenExceptionOccurs()
        {
            var dto = new CreateProductDto();

            _mockService.Setup(service => service.AddNewProduct(It.IsAny<CreateProductDto>())).Throws(new ProductNotFoundException("Product not found!"));

            var result = _controller.Create(dto);

            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            Assert.IsType<string>(conflictResult.Value);
        }

        [Fact]
        public void Update_ReturnsNoContentResult()
        {
            var dto = new UpdateProductDto();

            _mockService.Setup(service => service.UpdateProduct(It.IsAny<UpdateProductDto>()));

            var result = _controller.Update(dto);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void Update_ReturnsNotFoundResult_WhenProductNotFoundException()
        {
            var dto = new UpdateProductDto();

            _mockService.Setup(service => service.UpdateProduct(It.IsAny<UpdateProductDto>())).Throws(new ProductNotFoundException("Product not found!"));

            var result = _controller.Update(dto);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Product not found!", notFoundResult.Value);
        }

        [Fact]
        public void Update_ReturnsConflictResult_WhenExceptionOccurs()
        {
            var dto = new UpdateProductDto();

            _mockService.Setup(service => service.UpdateProduct(It.IsAny<UpdateProductDto>())).Throws<Exception>();

            var result = _controller.Update(dto);

            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            Assert.IsType<string>(conflictResult.Value);
        }

        [Fact]
        public void UpdatePrice_ReturnsNoContentResult()
        {
            var dto = new UpdateProductPriceDto();

            _mockService.Setup(service => service.UpdateProductPrice(It.IsAny<UpdateProductPriceDto>()));

            var result = _controller.UpdatePrice(dto);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void UpdatePrice_ReturnsNotFoundResult_WhenProductNotFoundException()
        {
            var dto = new UpdateProductPriceDto();
_mockService.Setup(service => service.UpdateProductPrice(It.IsAny<UpdateProductPriceDto>())).Throws(new ProductNotFoundException("Product not found!"));

            var result = _controller.UpdatePrice(dto);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Product not found!", notFoundResult.Value);
        }

        [Fact]
        public void UpdatePrice_ReturnsConflictResult_WhenExceptionOccurs()
        {
            var dto = new UpdateProductPriceDto();

            _mockService.Setup(service => service.UpdateProductPrice(It.IsAny<UpdateProductPriceDto>())).Throws<Exception>();

            var result = _controller.UpdatePrice(dto);

            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            Assert.IsType<Exception>(conflictResult.Value);
        }

        [Fact]
        public void UpdateAmount_ReturnsNoContentResult()
        {
            var dto = new UpdateProductAmountDto();

            _mockService.Setup(service => service.UpdateProductAmount(It.IsAny<UpdateProductAmountDto>()));

            var result = _controller.UpdateAmount(dto);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void UpdateAmount_ReturnsNotFoundResult_WhenProductNotFoundException()
        {
            var dto = new UpdateProductAmountDto();

            _mockService.Setup(service => service.UpdateProductAmount(It.IsAny<UpdateProductAmountDto>())).Throws(new ProductNotFoundException("Product not found!"));

            var result = _controller.UpdateAmount(dto);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(("{ message = Product not found! }"), notFoundResult.Value.ToString());
        }

        [Fact]
        public void UpdateAmount_ReturnsConflictResult_WhenExceptionOccurs()
        {
            var dto = new UpdateProductAmountDto();

            _mockService.Setup(service => service.UpdateProductAmount(It.IsAny<UpdateProductAmountDto>())).Throws<Exception>();

            var result = _controller.UpdateAmount(dto);

            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            Assert.IsType<string>(conflictResult.Value.ToString());
        }

        [Fact]
        public void Delete_ReturnsNoContentResult()
        {
            _mockService.Setup(service => service.DeleteProduct(It.IsAny<int>()));

            var result = _controller.Delete(1);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void Delete_ReturnsConflictResult_WhenExceptionOccurs()
        {
            _mockService.Setup(service => service.DeleteProduct(It.IsAny<int>())).Throws<Exception>();

            var result = _controller.Delete(1);

            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            Assert.IsType<string>(conflictResult.Value);
        }

        [Fact]
        public void Check_ReturnsOkResult_True()
        {
            _mockService.Setup(service => service.Check(It.IsAny<int>(), It.IsAny<int>())).Returns(true);

            var result = _controller.Check(1, 10);

            Assert.IsType<OkObjectResult>(result);
            Assert.True((bool)((OkObjectResult)result).Value);
        }

        [Fact]
        public void Check_ReturnsOkResult_False()
        {
            _mockService.Setup(service => service.Check(It.IsAny<int>(), It.IsAny<int>())).Returns(false);

            var result = _controller.Check(1, 10);

            Assert.IsType<OkObjectResult>(result);
            Assert.False((bool)((OkObjectResult)result).Value);
        }

        [Fact]
        public void Check_ReturnsConflictResult_WhenProductNotFoundException()
        {
            _mockService.Setup(service => service.Check(It.IsAny<int>(), It.IsAny<int>())).Throws(new ProductNotFoundException("Product not found!"));

            var result = _controller.Check(1, 10);

            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            Assert.IsType<string>(conflictResult.Value);
        }

        [Fact]
        public void Check_ReturnsConflictResult_WhenExceptionOccurs()
        {
            _mockService.Setup(service => service.Check(It.IsAny<int>(), It.IsAny<int>())).Throws<Exception>();

            var result = _controller.Check(1, 10);

            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            Assert.IsType<string>(conflictResult.Value);
        }
    }
}