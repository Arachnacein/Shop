using System;
using System.Linq;
using ClientManager.Data;
using ClientManager.Infrastructure.Repositories;
using ClientManager.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ClientManagerTests
{
    public class ClientRepositoryTests : IDisposable
    {
        private readonly DbContextOptions<ClientDbContext> _options;

        public ClientRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<ClientDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new ClientDbContext(_options))
            {
                context.Clients.Add(new Client { Id = Guid.NewGuid(), Name = "Client1", Surname = "Surname1", RegistryDate = DateTime.Now });
                context.Clients
                .Add(new Client { Id = Guid.NewGuid(), Name = "Client2", Surname = "Surname2", RegistryDate = DateTime.Now });
                context.SaveChanges();
            }
        }

        [Fact]
        public void GetClients_ReturnsAllClients()
        {
            // Arrange
            using (var context = new ClientDbContext(_options))
            {
                var repository = new ClientRepository(context);

                // Act
                var clients = repository.GetClients();

                // Assert
                Assert.Equal(2, clients.Count());
            }
        }

        [Fact]
        public void GetClient_ById_ReturnsClient()
        {
            // Arrange
            using (var context = new ClientDbContext(_options))
            {
                var repository = new ClientRepository(context);
                var clientId = context.Clients.First().Id;

                // Act
                var client = repository.GetClient(clientId);

                // Assert
                Assert.NotNull(client);
                Assert.Equal("Client1", client.Name);
            }
        }

        [Fact]
        public void GetClient_ByName_ReturnsClient()
        {
            // Arrange
            using (var context = new ClientDbContext(_options))
            {
                var repository = new ClientRepository(context);
                var clientName = "Client1";

                // Act
                var client = repository.GetClient(clientName);

                // Assert
                Assert.NotNull(client);
                Assert.Equal(clientName, client.Name);
            }
        }

        [Fact]
        public void AddClient_AddsClient()
        {
            // Arrange
            using (var context = new ClientDbContext(_options))
            {
                var repository = new ClientRepository(context);
                var newClient = new Client { Id = Guid.NewGuid(), Name = "Client3", Surname = "Surname3" };

                // Act
                var addedClient = repository.AddClient(newClient);

                // Assert
                Assert.NotNull(addedClient.RegistryDate);
                Assert.Equal("Client3", addedClient.Name);
            }
        }

        [Fact]
        public void UpdateClient_UpdatesClient()
        {
            // Arrange
            using (var context = new ClientDbContext(_options))
            {
                var repository = new ClientRepository(context);
                var clientToUpdate = context.Clients.First();
                clientToUpdate.Name = "UpdatedClient";

                // Act
                repository.UpdateClient(clientToUpdate);

                // Assert
                Assert.Equal("UpdatedClient", context.Clients.First(c => c.Id == clientToUpdate.Id).Name);
            }
        }

        [Fact]
        public void DeleteClient_DeletesClient()
        {
            // Arrange
            using (var context = new ClientDbContext(_options))
            {
                var repository = new ClientRepository(context);
                var clientId = context.Clients.First().Id;

                // Act
                repository.DeleteClient(clientId);

                // Assert
                Assert.DoesNotContain(context.Clients, c => c.Id == clientId);
            }
        }

        public void Dispose()
        {
            using (var context = new ClientDbContext(_options))
            {
                context.Database.EnsureDeleted();
            }
        }
    }
}
