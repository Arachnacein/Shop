using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using OrderManager.Data;
using OrderManager.Models;
using OrderManager.Repositories;
using OrderManager.Exceptions;
using Xunit;

namespace OrderManagerTests
{
    public class OrderRepositoryTests : IDisposable
    {
        private readonly DbContextOptions<OrderDbContext> _options;

        public OrderRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<OrderDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new OrderDbContext(_options))
            {
                context.Orders.Add(new Order { Id = 1, Id_Product = 1, Id_User = Guid.NewGuid(), Amount = 10, Price = 100m, CreateDate = DateTime.Now, Finished = false });
                context.Orders.Add(new Order { Id = 2, Id_Product = 2, Id_User = Guid.NewGuid(), Amount = 5, Price = 50m, CreateDate = DateTime.Now, Finished = true });
                context.SaveChanges();
            }
        }

        [Fact]
        public void GetAll_ReturnsAllOrders()
        {
            // Arrange
            using (var context = new OrderDbContext(_options))
            {
                var repository = new OrderRepository(context);

                // Act
                var orders = repository.GetAll();

                // Assert
                Assert.Equal(2, orders.Count());
            }
        }

        [Fact]
        public void GetById_WhenOrderExists_ReturnsOrder()
        {
            // Arrange
            using (var context = new OrderDbContext(_options))
            {
                var repository = new OrderRepository(context);
                var orderId = 1;

                // Act
                var order = repository.GetById(orderId);

                // Assert
                Assert.NotNull(order);
                Assert.Equal(orderId, order.Id);
            }
        }

        [Fact]
        public void GetById_WhenOrderDoesNotExist_ReturnsNull()
        {
            // Arrange
            using (var context = new OrderDbContext(_options))
            {
                var repository = new OrderRepository(context);
                var orderId = 3;

                // Act
                var order = repository.GetById(orderId);

                // Assert
                Assert.Null(order);
            }
        }

        [Fact]
        public void CreateOrder_AddsOrder()
        {
            // Arrange
            using (var context = new OrderDbContext(_options))
            {
                var repository = new OrderRepository(context);
                var newOrder = new Order { Id = 3, Id_Product = 3, Id_User = Guid.NewGuid(), Amount = 15, Price = 150m, CreateDate = DateTime.Now, Finished = false };

                // Act
                var addedOrder = repository.Create(newOrder);

                // Assert
                Assert.NotNull(addedOrder);
                Assert.Equal(3, addedOrder.Id_Product);
                Assert.Equal(15, addedOrder.Amount);
                Assert.Equal(150m, addedOrder.Price);
            }
        }

        [Fact]
        public void UpdateOrder_UpdatesOrder()
        {
            // Arrange
            using (var context = new OrderDbContext(_options))
            {
                var repository = new OrderRepository(context);
                var orderToUpdate = context.Orders.First();
                orderToUpdate.Amount = 20;

                // Act
                repository.Update(orderToUpdate);

                // Assert
                Assert.Equal(20, context.Orders.First(o => o.Id == orderToUpdate.Id).Amount);
            }
        }

        [Fact]
        public void UpdateOrder_WhenOrderDoesNotExist_ThrowsOrderNotFoundException()
        {
            // Arrange
            using (var context = new OrderDbContext(_options))
            {
                var repository = new OrderRepository(context);
                var nonExistingOrder = new Order { Id = 999, Id_Product = 1, Id_User = Guid.NewGuid(), Amount = 10, Price = 100m, CreateDate = DateTime.Now, Finished = false };

                // Act & Assert
                var exception = Assert.Throws<OrderNotFoundException>(() => repository.Update(nonExistingOrder));
                Assert.Equal("Order not found. Id: 999", exception.Message);
            }
        }

        [Fact]
        public void DeleteOrder_DeletesOrder()
        {
            // Arrange
            using (var context = new OrderDbContext(_options))
            {
                var repository = new OrderRepository(context);
                var orderId = 1;

                // Act
                repository.Delete(orderId);

                // Assert
                Assert.DoesNotContain(context.Orders, o => o.Id == orderId);
            }
        }

        [Fact]
        public void DeleteOrder_WhenOrderDoesNotExist_ThrowsOrderNotFoundException()
        {
            // Arrange
            using (var context = new OrderDbContext(_options))
            {
                var repository = new OrderRepository(context);
                var orderId = 999;

                // Act & Assert
                var exception = Assert.Throws<OrderNotFoundException>(() => repository.Delete(orderId));
                Assert.Equal("Order not found. Id: 999", exception.Message);
            }
        }

        [Fact]
        public void GetAllFinished_ReturnsFinishedOrders()
        {
            // Arrange
            using (var context = new OrderDbContext(_options))
            {
                var repository = new OrderRepository(context);

                // Act
                var orders = repository.GetAllFinished();

                // Assert
                Assert.Single(orders);
                Assert.True(orders.All(o => o.Finished));
            }
        }

        [Fact]
        public void GetAllUnfinished_ReturnsUnfinishedOrders()
        {
            // Arrange
            using (var context = new OrderDbContext(_options))
            {
                var repository = new OrderRepository(context);

                // Act
                var orders = repository.GetAllUnfinished();

                // Assert
                Assert.Single(orders);
                Assert.True(orders.All(o => !o.Finished));
            }
        }

        public void Dispose()
        {
            using (var context = new OrderDbContext(_options))
            {
                context.Database.EnsureDeleted();
            }
        }
    }
}
