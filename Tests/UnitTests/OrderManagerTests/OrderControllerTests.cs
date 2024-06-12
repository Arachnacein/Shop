using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using OrderManager.Controllers;
using OrderManager.Dto;
using OrderManager.Exceptions;
using OrderManager.Services;

namespace OrderManagerTests
{
    public class OrderControllerTests
    {
        private readonly Mock<IOrderService> _mockOrderService;
        private readonly OrderController _controller;

        public OrderControllerTests()
        {
            _mockOrderService = new Mock<IOrderService>();
            _controller = new OrderController(_mockOrderService.Object);
        }

                [Fact]
        public void Get_WhenOrdersExist_ReturnsOkResultWithOrders()
        {
            // Arrange
            var orders = new List<OrderDto> { new OrderDto(), new OrderDto() };
            _mockOrderService.Setup(service => service.GetAllOrders()).Returns(orders);

            // Act
            var result = _controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<OrderDto>>(okResult.Value);
            Assert.Equal(orders.Count, returnValue.Count);
        }

        [Fact]
        public void Get_WhenOrdersNotFound_ThrowsOrderNotFoundException()
        {
            // Arrange
            _mockOrderService.Setup(service => service.GetAllOrders()).Throws(new OrderNotFoundException("No orders found"));

            // Act
            var result = _controller.Get();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("No orders found", notFoundResult.Value.ToString());
        }
        [Fact]
        public void GetWithId_WhenOrderExists_ReturnsOkResultWithOrder()
        {
            // Arrange
            var orderId = 213213;
            var order = new OrderDto();
            _mockOrderService.Setup(service => service.GetOrderById(orderId)).Returns(order);

            // Act
            var result = _controller.Get(orderId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(order, okResult.Value);
        }

        [Fact]
        public void GetWithId_WhenOrderNotFound_ThrowsOrderNotFoundException()
        {
            // Arrange
            var orderId = 213213;
            _mockOrderService.Setup(service => service.GetOrderById(orderId)).Throws(new OrderNotFoundException("Order not found"));

            // Act
            var result = _controller.Get(orderId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Order not found", notFoundResult.Value);
        }
        [Fact]
        public void Create_WhenOrderIsCreated_ReturnsCreatedAtRouteResult()
        {
            // Arrange
            var newOrder = new CreateOrderDto();
            var order = new OrderDto { Id = 1 };
            _mockOrderService.Setup(service => service.CreateOrder(newOrder)).Returns(order);

            // Act
            var result = _controller.Create(newOrder);

            // Assert
            var createdAtRouteResult = Assert.IsType<CreatedResult>(result);
            Assert.Equal($"api/orders/{order.Id}", createdAtRouteResult.Location);
            Assert.Equal(order, createdAtRouteResult.Value);
        }
        ///
        [Fact]
        public void Update_WhenOrdertExists_ReturnsNoContentResult()
        {
            // Arrange
            var updateOrder = new UpdateOrderDto();
            _mockOrderService.Setup(service => service.UpdateOrder(updateOrder));

            // Act
            var result = _controller.Update(updateOrder);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void Update_WhenOrderNotFound_ThrowsOrderNotFoundException()
        {
            // Arrange
            var updateOrder = new UpdateOrderDto();
            _mockOrderService.Setup(service => service.UpdateOrder(updateOrder)).Throws(new OrderNotFoundException("Order not found"));

            // Act
            var result = _controller.Update(updateOrder);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Order not found", notFoundResult.Value);
        }

        [Fact]
        public void Delete_WhenOrderExists_ReturnsNoContentResult()
        {
            // Arrange
            var orderId = 1;
            _mockOrderService.Setup(service => service.DeleteOrder(orderId));

            // Act
            var result = _controller.Delete(orderId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void Delete_WhenOrderNotFound_ThrowOrderNotFoundException()
        {
            // Arrange
            var orderId = 1;
            _mockOrderService.Setup(service => service.DeleteOrder(orderId)).Throws(new OrderNotFoundException("Order not found"));

            // Act
            var result = _controller.Delete(orderId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Order not found", notFoundResult.Value);
        }
    }
}