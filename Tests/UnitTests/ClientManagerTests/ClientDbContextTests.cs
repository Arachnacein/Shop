using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Xunit;
using ClientManager.Data;
using ClientManager.Models;

namespace ClientManagerTests
{
    public class ClientDbContextUnitTests
    {
        private ClientDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ClientDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new ClientDbContext(options);
        }

        [Fact]
        public void AddClient_AddsClientToDatabase()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var client = new Client
            {
                Id = Guid.NewGuid(),
                Name = "John",
                Surname = "Doe",
                RegistryDate = DateTime.Now
            };

            // Act
            dbContext.Clients.Add(client);
            dbContext.SaveChanges();

            // Assert
            var savedClient = dbContext.Clients.FirstOrDefault(c => c.Id == client.Id);
            Assert.NotNull(savedClient);
            Assert.Equal(client.Name, savedClient.Name);
            Assert.Equal(client.Surname, savedClient.Surname);
            Assert.Equal(client.RegistryDate, savedClient.RegistryDate);
        }

        [Fact]
        public void GetClientById_ReturnsClient()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var clientId = Guid.NewGuid();
            var client = new Client
            {
                Id = clientId,
                Name = "John",
                Surname = "Doe",
                RegistryDate = DateTime.Now
            };
            dbContext.Clients.Add(client);
            dbContext.SaveChanges();

            // Act
            var retrievedClient = dbContext.Clients.Find(clientId);

            // Assert
            Assert.NotNull(retrievedClient);
            Assert.Equal(clientId, retrievedClient.Id);
        }

        [Fact]
        public void GetAllClients_ReturnsAllClients()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            dbContext.Clients.AddRange(
                new Client { Id = Guid.NewGuid(), Name = "John", Surname = "Doe", RegistryDate = DateTime.Now },
                new Client { Id = Guid.NewGuid(), Name = "Jane", Surname = "Smith", RegistryDate = DateTime.Now }
            );
            dbContext.SaveChanges();

            // Act
            var clients = dbContext.Clients.ToList();

            // Assert
            Assert.Equal(2, clients.Count);
        }

        [Fact]
        public void UpdateClient_UpdatesClientInDatabase()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var client = new Client
            {
                Id = Guid.NewGuid(),
                Name = "John",
                Surname = "Doe",
                RegistryDate = DateTime.Now
            };
            dbContext.Clients.Add(client);
            dbContext.SaveChanges();

            // Act
            client.Name = "Johnny";
            dbContext.Clients.Update(client);
            dbContext.SaveChanges();

            // Assert
            var updatedClient = dbContext.Clients.Find(client.Id);
            Assert.NotNull(updatedClient);
            Assert.Equal("Johnny", updatedClient.Name);
        }

        [Fact]
        public void DeleteClient_DeletesClientFromDatabase()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var client = new Client
            {
                Id = Guid.NewGuid(),
                Name = "John",
                Surname = "Doe",
                RegistryDate = DateTime.Now
            };
            dbContext.Clients.Add(client);
            dbContext.SaveChanges();

            // Act
            dbContext.Clients.Remove(client);
            dbContext.SaveChanges();

            // Assert
            var deletedClient = dbContext.Clients.Find(client.Id);
            Assert.Null(deletedClient);
        }
    }
}
