using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using ClientManager.Controllers;
using ClientManager.Dtos;
using ClientManager.Exceptions;
using ClientManager.Services;

namespace ClientManagerTests
{
    public class ClientControllerTests
    {
        private readonly Mock<IClientService> _mockClientService;
        private readonly ClientController _controller;

        public ClientControllerTests()
        {
            _mockClientService = new Mock<IClientService>();
            _controller = new ClientController(_mockClientService.Object);
        }

        [Fact]
        public void Get_WhenClientsExist_ReturnsOkResultWithClients()
        {
            // Arrange
            var clients = new List<ClientDto> { new ClientDto(), new ClientDto() };
            _mockClientService.Setup(service => service.GetAllClients()).Returns(clients);

            // Act
            var result = _controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<ClientDto>>(okResult.Value);
            Assert.Equal(clients.Count, returnValue.Count);
        }

        [Fact]
        public void Get_WhenClientsNotFound_ThrowsClientNotFoundException()
        {
            // Arrange
            _mockClientService.Setup(service => service.GetAllClients()).Throws(new ClientNotFoundException("No clients found"));

            // Act
            var result = _controller.Get();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("No clients found", notFoundResult.Value.ToString());
        }

        [Fact]
        public void GetById_WhenClientExists_ReturnsOkResultWithClient()
        {
            // Arrange
            var clientId = Guid.NewGuid();
            var client = new ClientDto();
            _mockClientService.Setup(service => service.GetClient(clientId)).Returns(client);

            // Act
            var result = _controller.GetById(clientId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(client, okResult.Value);
        }

        [Fact]
        public void GetById_WhenClientNotFound_ThrowsClientNotFoundException()
        {
            // Arrange
            var clientId = Guid.NewGuid();
            _mockClientService.Setup(service => service.GetClient(clientId)).Throws(new ClientNotFoundException("Client not found"));

            // Act
            var result = _controller.GetById(clientId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Client not found", notFoundResult.Value);
        }

        [Fact]
        public void Create_WhenClientIsCreated_ReturnsCreatedAtRouteResult()
        {
            // Arrange
            var newClient = new CreateClientDto();
            var client = new ClientDto { Id = Guid.NewGuid() };
            _mockClientService.Setup(service => service.AddNewClient(newClient)).Returns(client);

            // Act
            var result = _controller.Create(newClient);

            // Assert
            var createdAtRouteResult = Assert.IsType<CreatedResult>(result);
            Assert.Equal($"api/clients/{client.Id}", createdAtRouteResult.Location);
            Assert.Equal(client, createdAtRouteResult.Value);
        }

        [Fact]
        public void Update_WhenClientExists_ReturnsNoContentResult()
        {
            // Arrange
            var updateClient = new UpdateClientDto();
            _mockClientService.Setup(service => service.UpdateClient(updateClient));

            // Act
            var result = _controller.Update(updateClient);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void Update_WhenClientNotFound_ThrowsClientNotFoundException()
        {
            // Arrange
            var updateClient = new UpdateClientDto();
            _mockClientService.Setup(service => service.UpdateClient(updateClient)).Throws(new ClientNotFoundException("Client not found"));

            // Act
            var result = _controller.Update(updateClient);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Client not found", notFoundResult.Value);
        }

        [Fact]
        public void Delete_WhenClientExists_ReturnsNoContentResult()
        {
            // Arrange
            var clientId = Guid.NewGuid();
            _mockClientService.Setup(service => service.DeleteClient(clientId));

            // Act
            var result = _controller.Delete(clientId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void Delete_WhenClientNotFound_ThrowsClientNotFoundException()
        {
            // Arrange
            var clientId = Guid.NewGuid();
            _mockClientService.Setup(service => service.DeleteClient(clientId)).Throws(new ClientNotFoundException("Client not found"));

            // Act
            var result = _controller.Delete(clientId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Client not found", notFoundResult.Value);
        }
    }
}
