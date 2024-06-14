using System;
using System.Collections.Generic;
using Moq;
using Xunit;
using AutoMapper;
using OrderManager.Data;
using OrderManager.Exceptions;
using OrderManager.Services;
using OrderManager.Mappers;
using OrderManager.Models;
using OrderManager.Dto;

namespace OrderManagerTests
{
    public class OrderServiceTests
    {
        private readonly Mock<IOrderRepository> _mockOrderRepository;
        private readonly Mock<IOrderMapper> _mockOrderMapper;
        private readonly Mock<IMapper> _mockMapper;
        private readonly OrderService _orderService;

        public OrderServiceTests()
        {
            _mockOrderRepository = new Mock<IOrderRepository>();
            _mockOrderMapper = new Mock<IOrderMapper>();
            _mockMapper = new Mock<IMapper>();
            _orderService = new OrderService(_mockOrderRepository.Object, _mockMapper.Object, _mockOrderMapper.Object);
        }

        [Fact]
        public void GetOrderById_WhenOrderExists_ReturnsOrderDto()
        {
            // Arrange
            var orderId = 1;
            var order = new Order();
            var orderDto = new OrderDto();
            _mockOrderRepository.Setup(repo => repo.GetById(orderId)).Returns(order);
            _mockOrderMapper.Setup(mapper => mapper.Map(order)).Returns(orderDto);

            // Act
            var result = _orderService.GetOrderById(orderId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(orderDto, result);
        }

        [Fact]
        public void GetOrderById_WhenOrderDoesNotExist_ThrowsOrderNotFoundException()
        {
            // Arrange
            var orderId = 1;
            _mockOrderRepository.Setup(repo => repo.GetById(orderId)).Returns((Order)null);

            // Act
            var exception = Assert.Throws<OrderNotFoundException>(() => _orderService.GetOrderById(orderId));
            
            // Assert
            Assert.Equal($"Order not found. Id:{orderId}", exception.Message);
        }

        [Fact]
        public void GetAllOrders_WhenOrdersExist_ReturnsOrderDtos()
        {
            // Arrange
            var orders = new List<Order> { new Order(), new Order() };
            var orderDtos = new List<OrderDto> { new OrderDto(), new OrderDto() };
            _mockOrderRepository.Setup(repo => repo.GetAll()).Returns(orders);
            _mockOrderMapper.Setup(mapper => mapper.MapElements(orders)).Returns(orderDtos);

            // Act
            var result = _orderService.GetAllOrders();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(orderDtos, result);
        }

        [Fact]
        public void CreateOrder_ValidOrder_ReturnsOrderDto()
        {
            // Arrange
            var createOrderDto = new CreateOrderDto()
            {
                Amount = 2,
                Price = 1.0m
            };
            var order = new Order();
            var orderDto = new OrderDto();
            _mockMapper.Setup(mapper => mapper.Map<Order>(createOrderDto)).Returns(order);
            _mockOrderMapper.Setup(mapper => mapper.Map(order)).Returns(orderDto);

            // Act
            var result = _orderService.CreateOrder(createOrderDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(orderDto, result);
        }

        [Fact]
        public void UpdateOrder_WhenOrderExists_UpdatesOrder()
        {
            // Arrange
            var updateOrderDto = new UpdateOrderDto 
            { 
                Id = 1, 
                Amount = 10,
                Price = 9.99m 
            };
            var existingOrder = new Order { Id = 1, CreateDate = DateTime.Now };
            _mockOrderRepository.Setup(repo => repo.GetById(updateOrderDto.Id)).Returns(existingOrder);
            _mockMapper.Setup(mapper => mapper.Map<Order>(updateOrderDto)).Returns(existingOrder);

            // Act
            _orderService.UpdateOrder(updateOrderDto);

            // Assert
            _mockOrderRepository.Verify(repo => repo.Update(It.IsAny<Order>()), Times.Once);
        }

        [Fact]
        public void UpdateOrder_WhenOrderDoesNotExist_ThrowsOrderNotFoundException()
        {
            // Arrange
            var updateOrderDto = new UpdateOrderDto 
            { 
                Id = 1, 
                Amount = 10,
                Price = 9.99m 
            };
            _mockOrderRepository.Setup(repo => repo.GetById(updateOrderDto.Id)).Returns((Order)null);

            // Act & Assert
            var exception = Assert.Throws<OrderNotFoundException>(() => _orderService.UpdateOrder(updateOrderDto));
            Assert.Equal($"Order not found id:{updateOrderDto.Id}", exception.Message);
        }

        [Fact]
        public void FinishOrder_WhenOrderExists_FinishesOrder()
        {
            // Arrange
            var orderId = 1;
            var existingOrder = new Order { Id = orderId, Finished = false };
            _mockOrderRepository.Setup(repo => repo.GetById(orderId)).Returns(existingOrder);

            // Act
            _orderService.FinishOrder(orderId);

            // Assert
            _mockOrderRepository.Verify(repo => repo.Update(It.Is<Order>(o => o.Finished == true && o.CompletionDate != DateTime.MinValue)), Times.Once);
        }

        [Fact]
        public void FinishOrder_WhenOrderDoesNotExist_ThrowsOrderNotFoundException()
        {
            // Arrange
            var orderId = 1;
            _mockOrderRepository.Setup(repo => repo.GetById(orderId)).Returns((Order)null);

            // Act & Assert
            var exception = Assert.Throws<OrderNotFoundException>(() => _orderService.FinishOrder(orderId));
            Assert.Equal($"Order not found id:{orderId}", exception.Message);
        }

        [Fact]
        public void DeleteOrder_WhenOrderExists_DeletesOrder()
        {
            // Arrange
            var orderId = 1;
            var existingOrder = new Order { Id = orderId };
            _mockOrderRepository.Setup(repo => repo.GetById(orderId)).Returns(existingOrder);

            // Act
            _orderService.DeleteOrder(orderId);

            // Assert
            _mockOrderRepository.Verify(repo => repo.Delete(orderId), Times.Once);
        }

        [Fact]
        public void DeleteOrder_WhenOrderDoesNotExist_ThrowsOrderNotFoundException()
        {
            // Arrange
            var orderId = 1;
            _mockOrderRepository.Setup(repo => repo.GetById(orderId)).Returns((Order)null);

            // Act & Assert
            var exception = Assert.Throws<OrderNotFoundException>(() => _orderService.DeleteOrder(orderId));
            Assert.Equal($"Order not found id:{orderId}", exception.Message);
        }
    }
}
