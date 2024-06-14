using System;
using Moq;
using Xunit;
using AutoMapper;
using ClientManager.Data.Repositories;
using ClientManager.Exceptions;
using ClientManager.Services;
using ClientManager.Mappers;
using ClientManager.Models;
using ClientManager.Dtos;

namespace ClientManagerTests;

public class ClientServiceTests
{
    private readonly Mock<IClientRepository> _mockClientRepository;
    private readonly Mock<IClientMapper> _mockClientMapper;
    private readonly Mock<IMapper> _mockMapper;
    private readonly ClientService _clientService;

    public ClientServiceTests()
    {
        _mockClientRepository = new Mock<IClientRepository>();
        _mockClientMapper = new Mock<IClientMapper>();
        _mockMapper = new Mock<IMapper>();
        _clientService = new ClientService(_mockClientRepository.Object, _mockClientMapper.Object, _mockMapper.Object);
    }

    [Fact]
    public void GetClient_WhenClientExists_ReturnsClientDto()
    {
        //Arrange
        var clientId = Guid.NewGuid();
        var client = new Client(); 
        var clientDto = new ClientDto();
        _mockClientRepository.Setup(repo => repo.GetClient(clientId)).Returns(client);
        _mockClientMapper.Setup(mapper => mapper.Map(client)).Returns(clientDto);

        //Act
        var result = _clientService.GetClient(clientId);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(clientDto, result);
    }

    [Fact]
    public void GetClient_WhenClientDoesNotExist_ThrowsClientNotFoundException()
    {
        //Arrange
        var clientId = Guid.NewGuid();
        _mockClientRepository.Setup(repo => repo.GetClient(clientId)).Returns((Client)null);

        //Act
        var exception = Assert.Throws<ClientNotFoundException>(() => _clientService.GetClient(clientId));
        
        //Assert
        Assert.Equal($"Client not found. Id:{clientId}", exception.Message);
    }
    [Fact]
    public void GetAllClients_WhenClientsExist_ReturnsClientDtos()
    {
        // Arrange
        var clients = new List<Client>();
        clients.Add(new Client());
        clients.Add(new Client());
        _mockClientRepository.Setup(repo => repo.GetClients()).Returns(clients);
        _mockClientMapper.Setup(mapper => mapper.MapElements(clients)).Returns(new List<ClientDto>());
        // Act
        var result = _clientService.GetAllClients();
        // Assert
        Assert.NotNull(result);
        Assert.Empty(result); // assuming mapper returns an empty list
    }
    [Fact]
    public void GetAllClients_WhenNoClientsExist_ThrowsArgumentNullException()
    {
        // Arrange
        _mockClientRepository.Setup(repo => repo.GetClients()).Returns(new List<Client>());
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _clientService.GetAllClients());
    }
    [Fact]
    public void AddNewClient_ValidClient_ReturnsClientDto()
    {
        // Arrange
        var createClientDto = new CreateClientDto
        {
            Name = "Grażyna", 
            Surname = "Babacka" 
        };
        var client = new Client();
        var clientDto = new ClientDto();
        _mockMapper.Setup(mapper => mapper.Map<Client>(createClientDto)).Returns(client);
        _mockClientMapper.Setup(mapper => mapper.Map(client)).Returns(clientDto);
        // Act
        var result = _clientService.AddNewClient(createClientDto);
        // Assert
        Assert.NotNull(result);
        Assert.Equal(clientDto, result);
    }
    [Fact]
    public void UpdateClient_WhenClientExists_UpdatesClient()
    {
        // Arrange
        var updateClientDto = new UpdateClientDto
        {
            Id = Guid.Empty, 
            Name = "Grażyna", 
            Surname = "Babacka" 
        };
        var existingClient = new Client();
        _mockClientRepository.Setup(repo => repo.GetClient(updateClientDto.Id)).Returns(existingClient);
        // Act
        _clientService.UpdateClient(updateClientDto);
        // Assert
        _mockClientRepository.Verify(repo => repo.UpdateClient(It.IsAny<Client>()), Times.Once);
    }
    [Fact]
    public void UpdateClient_WhenClientDoesNotExist_ThrowsClientNotFoundException()
    {
        // Arrange
        var updateClientDto = new UpdateClientDto
        {
            Id = Guid.Empty, 
            Name = "Grażyna", 
            Surname = "Babacka" 
        };
        _mockClientRepository.Setup(repo => repo.GetClient(updateClientDto.Id)).Returns((Client)null);
        // Act & Assert
        var exception = Assert.Throws<ClientNotFoundException>(() => _clientService.UpdateClient(updateClientDto));
        Assert.Equal($"Client not found. Id:{updateClientDto.Id}", exception.Message);
    }
    [Fact]
    public void DeleteClient_WhenClientExists_DeletesClient()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var existingClient = new Client();
        _mockClientRepository.Setup(repo => repo.GetClient(clientId)).Returns(existingClient);
        // Act
        _clientService.DeleteClient(clientId);
        // Assert
        _mockClientRepository.Verify(repo => repo.DeleteClient(clientId), Times.Once);
    }
    [Fact]
    public void DeleteClient_WhenClientDoesNotExist_ThrowsClientNotFoundException()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        _mockClientRepository.Setup(repo => repo.GetClient(clientId)).Returns((Client)null);
        // Act & Assert
        var exception = Assert.Throws<ClientNotFoundException>(() => _clientService.DeleteClient(clientId));
        Assert.Equal($"Client not found. Id:{clientId}", exception.Message);
    }
    [Fact]
    public void CountClients_ReturnsNumberOfClients()
    {
        // Arrange
        var clients = new List<Client>();
        clients.Add(new Client());
        clients.Add(new Client());
        _mockClientRepository.Setup(repo => repo.GetClients()).Returns(clients);
        // Act
        var result = _clientService.CountClients();
        // Assert
        Assert.Equal(clients.Count, result);
    }
}