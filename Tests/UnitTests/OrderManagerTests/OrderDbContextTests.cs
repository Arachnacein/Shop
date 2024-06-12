using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Xunit;
using OrderManager.Data;
using OrderManager.Models;

namespace OrderManagerTests
{
    public class OrderDbContextUnitTests
    {
        private OrderDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<OrderDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new OrderDbContext(options);
        }

        [Fact]
        public void AddOrder_AddsOrderToDatabase()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var order = new Order
            {
                Id_Product = 1,
                Id_User = Guid.NewGuid(),
                Amount = 10,
                Price = 100.0m,
                CreateDate = DateTime.Now,
                CompletionDate = DateTime.Now.AddDays(7),
                Finished = false
            };

            // Act
            dbContext.Orders.Add(order);
            dbContext.SaveChanges();

            // Assert
            var savedOrder = dbContext.Orders.FirstOrDefault(o => o.Id == order.Id);
            Assert.NotNull(savedOrder);
            Assert.Equal(order.Id_Product, savedOrder.Id_Product);
            Assert.Equal(order.Id_User, savedOrder.Id_User);
            Assert.Equal(order.Amount, savedOrder.Amount);
            Assert.Equal(order.Price, savedOrder.Price);
            Assert.Equal(order.CreateDate, savedOrder.CreateDate);
            Assert.Equal(order.CompletionDate, savedOrder.CompletionDate);
            Assert.Equal(order.Finished, savedOrder.Finished);
        }

        [Fact]
        public void GetOrderById_ReturnsOrder()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var orderId = 1;
            var order = new Order
            {
                Id = orderId,
                Id_Product = 1,
                Id_User = Guid.NewGuid(),
                Amount = 10,
                Price = 100.0m,
                CreateDate = DateTime.Now,
                CompletionDate = DateTime.Now.AddDays(7),
                Finished = false
            };
            dbContext.Orders.Add(order);
            dbContext.SaveChanges();

            // Act
            var retrievedOrder = dbContext.Orders.Find(orderId);

            // Assert
            Assert.NotNull(retrievedOrder);
            Assert.Equal(orderId, retrievedOrder.Id);
        }

        [Fact]
        public void GetAllOrders_ReturnsAllOrders()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            dbContext.Orders.AddRange(
                new Order
                {
                    Id_Product = 1,
                    Id_User = Guid.NewGuid(),
                    Amount = 10,
                    Price = 100.0m,
                    CreateDate = DateTime.Now,
                    CompletionDate = DateTime.Now.AddDays(7),
                    Finished = false
                },
                new Order
                {
                    Id_Product = 2,
                    Id_User = Guid.NewGuid(),
                    Amount = 5,
                    Price = 50.0m,
                    CreateDate = DateTime.Now,
                    CompletionDate = DateTime.Now.AddDays(7),
                    Finished = false
                }
            );
            dbContext.SaveChanges();

            // Act
            var orders = dbContext.Orders.ToList();

            // Assert
            Assert.Equal(2, orders.Count);
        }

        [Fact]
        public void UpdateOrder_UpdatesOrderInDatabase()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var order = new Order
            {
                Id_Product = 1,
                Id_User = Guid.NewGuid(),
                Amount = 10,
                Price = 100.0m,
                CreateDate = DateTime.Now,
                CompletionDate = DateTime.Now.AddDays(7),
                Finished = false
            };
            dbContext.Orders.Add(order);
            dbContext.SaveChanges();

            // Act
            order.Amount = 20;
            dbContext.Orders.Update(order);
            dbContext.SaveChanges();

            // Assert
            var updatedOrder = dbContext.Orders.Find(order.Id);
            Assert.NotNull(updatedOrder);
            Assert.Equal(20, updatedOrder.Amount);
        }

        [Fact]
        public void DeleteOrder_DeletesOrderFromDatabase()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var order = new Order
            {
                Id_Product = 1,
                Id_User = Guid.NewGuid(),
                Amount = 10,
                Price = 100.0m,
                CreateDate = DateTime.Now,
                CompletionDate = DateTime.Now.AddDays(7),
                Finished = false
            };
            dbContext.Orders.Add(order);
            dbContext.SaveChanges();

            // Act
            dbContext.Orders.Remove(order);
            dbContext.SaveChanges();

            // Assert
            var deletedOrder = dbContext.Orders.Find(order.Id);
            Assert.Null(deletedOrder);
        }
    }
}
