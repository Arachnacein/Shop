using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using UI.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net;

namespace ClientManagerTests
{    
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class ClientPageTests : PageTest
    {
        private readonly string apiBaseUrl = "http://localhost:8010/api/client";

        [Test]
        public async Task GetClientList_WhenCalled_ShouldReturnNonEmptyListOfClients()
        {
            // Arrange
            var httpClient = new HttpClient();

            // Act
            var response = await httpClient.GetAsync(apiBaseUrl);
            response.EnsureSuccessStatusCode();

            var clients = await response.Content.ReadFromJsonAsync<List<ClientViewModel>>();

            // Assert
            Assert.IsNotNull(clients);
            Assert.IsTrue(clients.Count > 0, "Client list should not be empty.");
        }
        [Test]
        public async Task UpdateClient_WhenDataIsValid_ShouldUpdateClientInformation()
        {
            // Arrange
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(apiBaseUrl);
            response.EnsureSuccessStatusCode();
            var clients = await response.Content.ReadFromJsonAsync<List<ClientViewModel>>();
           
            var clientToUpdate = clients[0]; 
            var updatedClient = new ClientViewModel
            {
                Id = clientToUpdate.Id,
                Name = "NewName",
                Surname = "NewSurname"
            };

            // Act
            var updateResponse = await httpClient.PutAsJsonAsync(apiBaseUrl, updatedClient);
            updateResponse.EnsureSuccessStatusCode();
            var getUpdatedResponse = await httpClient.GetAsync($"{apiBaseUrl}/{clientToUpdate.Id}");
            getUpdatedResponse.EnsureSuccessStatusCode();
            var updatedClientResponse = await getUpdatedResponse.Content.ReadFromJsonAsync<ClientViewModel>();

            // Assert
            Assert.IsNotNull(updatedClientResponse);
            Assert.AreEqual(updatedClient.Name, updatedClientResponse.Name);
            Assert.AreEqual(updatedClient.Surname, updatedClientResponse.Surname);
            Assert.AreEqual(updatedClient.Id, updatedClientResponse.Id);
        }
        [Test]
        public async Task UpdateClient_WhenClientDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            var httpClient = new HttpClient();
            var updatedClient = new ClientViewModel
            {
                Id = Guid.Empty,
                Name = "NewName",
                Surname = "NewSurname"
            };

            // Act
            var updateResponse = await httpClient.PutAsJsonAsync(apiBaseUrl, updatedClient);

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, updateResponse.StatusCode);
        }
        [Test]
        public async Task DeleteClient_WhenClientExists_ShouldDeleteClient()
        {
            // Arrange
            var httpClient = new HttpClient();

            var response = await httpClient.GetAsync(apiBaseUrl);
            response.EnsureSuccessStatusCode();
            var clients = await response.Content.ReadFromJsonAsync<List<ClientViewModel>>();
            var clientToDelete = clients[0]; 

            // Act
            var deleteResponse = await httpClient.DeleteAsync($"{apiBaseUrl}/{clientToDelete.Id}");
            deleteResponse.EnsureSuccessStatusCode();
            var getResponse = await httpClient.GetAsync($"{apiBaseUrl}/{clientToDelete.Id}");

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, getResponse.StatusCode);
        }
        [Test]
        public async Task DeleteClient_WhenClientDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            var httpClient = new HttpClient();
            var nonExistentClientId = "non_existent_client_id";

            // Act
            var deleteResponse = await httpClient.DeleteAsync($"{apiBaseUrl}/{nonExistentClientId}");

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, deleteResponse.StatusCode);
        }
        [Test]
        public async Task AddClient_WithValidData_ShouldAddClient()
        {
            // Arrange
            var httpClient = new HttpClient();
            var newClient = new ClientViewModel
            {
                Name = "NewName",
                Surname = "NewSurname"
            };

            // Act
            var response = await httpClient.PostAsJsonAsync(apiBaseUrl, newClient);
            response.EnsureSuccessStatusCode(); // Upewnij się, że odpowiedź jest sukcesem

            var clients = await httpClient.GetAsync(apiBaseUrl);
            var clientsList = await clients.Content.ReadFromJsonAsync<List<ClientViewModel>>();
            var freshClient = clientsList.Last(); // Pobierz ostatniego dodanego klienta

            // Assert
            Assert.AreEqual(newClient.Name, freshClient.Name);
            Assert.AreEqual(newClient.Surname, freshClient.Surname);           
        }
    }
}