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

namespace UITests
{    
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class OrderPageTests : PageTest
    {
        private readonly string apiBaseUrlClient = "http://localhost:8010/api/client";
        private readonly string apiBaseUrlProduct = "http://localhost:8010/api/product";
        private readonly string apiBaseUrlOrder = "http://localhost:8010/api/order";

        [Test]
        public async Task AddOrder_WithValidData_ShouldAddOrderCorrectly()
        {
            // Arrange
            var httpClient = new HttpClient();

            var users = await httpClient.GetAsync(apiBaseUrlClient);
            var usersList = await users.Content.ReadFromJsonAsync<List<ClientViewModel>>();
            var user = usersList.First();

            var products = await httpClient.GetAsync(apiBaseUrlProduct);
            var productsList = await products.Content.ReadFromJsonAsync<List<ProductViewModel>>();
            var product = productsList.First();

            var newOrder = new 
            {
                Id_User = user.Id,
                Id_Product = product.Id,
                Amount = 20,
                Price = 19.99m
            };

            // Act
            var response = await httpClient.PostAsJsonAsync(apiBaseUrlOrder, newOrder);
            response.EnsureSuccessStatusCode(); 

            var orders = await httpClient.GetAsync(apiBaseUrlOrder);
            var ordersList = await orders.Content.ReadFromJsonAsync<List<OrderViewModel>>();
            var freshOrder = ordersList.Last(); 

            // Assert
            Assert.AreEqual(newOrder.Id_Product, freshOrder.Id_Product);
            Assert.AreEqual(newOrder.Id_User, freshOrder.Id_User);           
            Assert.AreEqual(newOrder.Price, freshOrder.Price);   
            Assert.AreEqual(newOrder.Amount, freshOrder.Amount);  
            Assert.NotNull(freshOrder.CreateDate);
            Assert.NotNull(freshOrder.CompletionDate);
            Assert.NotNull(freshOrder.Finished);
            Assert.IsFalse(freshOrder.Finished);
        }
        [Test]
        public async Task AddOrder_WithInvalidAmount_ShouldNotAddOrder()
        {        
            // Arrange
            var httpClient = new HttpClient();

            var users = await httpClient.GetAsync(apiBaseUrlClient);
            var usersList = await users.Content.ReadFromJsonAsync<List<ClientViewModel>>();
            var user = usersList.First();

            var products = await httpClient.GetAsync(apiBaseUrlProduct);
            var productsList = await products.Content.ReadFromJsonAsync<List<ProductViewModel>>();
            var product = productsList.First();

            var newOrder = new 
            {
                Id_User = user.Id,
                Id_Product = product.Id,
                Amount = -20,
                Price = 15.90m
            };

            // Act
            var response = await httpClient.PostAsJsonAsync(apiBaseUrlOrder, newOrder);

            // Assert
            Assert.AreEqual(HttpStatusCode.Conflict, response.StatusCode);
        }
        [Test]
        public async Task AddOrder_WithInvalidPrice_ShouldNotAddOrder()
        {        
            // Arrange
            var httpClient = new HttpClient();

            var users = await httpClient.GetAsync(apiBaseUrlClient);
            var usersList = await users.Content.ReadFromJsonAsync<List<ClientViewModel>>();
            var user = usersList.First();

            var products = await httpClient.GetAsync(apiBaseUrlProduct);
            var productsList = await products.Content.ReadFromJsonAsync<List<ProductViewModel>>();
            var product = productsList.First();

            var newOrder = new 
            {
                Id_User = user.Id,
                Id_Product = product.Id,
                Amount = 20,
                Price = -15.90m
            };

            // Act
            var response = await httpClient.PostAsJsonAsync(apiBaseUrlOrder, newOrder);

            // Assert
            Assert.AreEqual(HttpStatusCode.Conflict, response.StatusCode);
        }
        [Test]
        public async Task GetOrderList_WhenCalled_ShouldReturnNonEmptyListOfOrders()
        {
            // Arrange
            var httpClient = new HttpClient();
            var newOrder = new 
            {
                Id_User = Guid.Empty,
                Id_Product = 2,
                Amount = 999,
                Price = 19.99m
            };

            await httpClient.PostAsJsonAsync(apiBaseUrlOrder, newOrder);
            //Act
            var response = await httpClient.GetAsync(apiBaseUrlOrder);
            response.EnsureSuccessStatusCode();

            var orders = await response.Content.ReadFromJsonAsync<List<OrderViewModel>>();

            // Assert
            Assert.IsNotNull(orders);
            Assert.IsTrue(orders.Count > 0, "Client list should not be empty.");          
        }
        [Test]
        public async Task UpdateOrder_WhenDataIsValid_ShouldUpdateOrderInformation()
        {
            // Arrange
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(apiBaseUrlOrder);
            response.EnsureSuccessStatusCode();
            var orders = await response.Content.ReadFromJsonAsync<List<OrderViewModel>>();
            var orderToUpdate = orders.Last(); 

            var users = await httpClient.GetAsync(apiBaseUrlClient);
            var usersList = await users.Content.ReadFromJsonAsync<List<ClientViewModel>>();
            var user = usersList.First();

            var products = await httpClient.GetAsync(apiBaseUrlProduct);
            var productsList = await products.Content.ReadFromJsonAsync<List<ProductViewModel>>();
            var product = productsList.First();
            
            var updatedOrder = new OrderViewModel
            {
                Id = orderToUpdate.Id,
                Id_Product = product.Id,
                Id_User = user.Id,
                Amount = 555,
                Price = 5.99m,
                CompletionDate = DateTime.Now,
                Finished = false
            };

            // Act
            var updateResponse = await httpClient.PutAsJsonAsync(apiBaseUrlOrder, updatedOrder);
            updateResponse.EnsureSuccessStatusCode();
            var getUpdatedResponse = await httpClient.GetAsync($"{apiBaseUrlOrder}/{orderToUpdate.Id}");
            getUpdatedResponse.EnsureSuccessStatusCode();
            var updatedOrderResponse = await getUpdatedResponse.Content.ReadFromJsonAsync<OrderViewModel>();

            // Assert
            Assert.IsNotNull(updatedOrderResponse);
            Assert.AreEqual(updatedOrder.Id, updatedOrderResponse.Id);          
        }
        [Test]
        public async Task UpdateOrder_WhenInvalidAmount_ShouldNotUpdateOrderInformation()
        {
            // Arrange
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(apiBaseUrlOrder);
            response.EnsureSuccessStatusCode();
            var orders = await response.Content.ReadFromJsonAsync<List<OrderViewModel>>();
            var orderToUpdate = orders.Last(); 

            var users = await httpClient.GetAsync(apiBaseUrlClient);
            var usersList = await users.Content.ReadFromJsonAsync<List<ClientViewModel>>();
            var user = usersList.First();

            var products = await httpClient.GetAsync(apiBaseUrlProduct);
            var productsList = await products.Content.ReadFromJsonAsync<List<ProductViewModel>>();
            var product = productsList.First();   

            var updatedOrder = new OrderViewModel
            {
                Id = orderToUpdate.Id,
                Id_Product = product.Id,
                Id_User = user.Id,
                Amount = -555,
                Price = 5.99m,
                CompletionDate = DateTime.Now,
                Finished = false
            };

            // Act
            var updateResponse = await httpClient.PutAsJsonAsync(apiBaseUrlOrder, updatedOrder);

            // Assert
            Assert.AreEqual(HttpStatusCode.Conflict, updateResponse.StatusCode);          
        }
        [Test]
        public async Task UpdateOrder_WhenInvalidPrice_ShouldNotUpdateOrderInformation()
        {
            // Arrange
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(apiBaseUrlOrder);
            response.EnsureSuccessStatusCode();
            var orders = await response.Content.ReadFromJsonAsync<List<OrderViewModel>>();
            var orderToUpdate = orders.Last(); 

            var users = await httpClient.GetAsync(apiBaseUrlClient);
            var usersList = await users.Content.ReadFromJsonAsync<List<ClientViewModel>>();
            var user = usersList.First();

            var products = await httpClient.GetAsync(apiBaseUrlProduct);
            var productsList = await products.Content.ReadFromJsonAsync<List<ProductViewModel>>();
            var product = productsList.First();   

            var updatedOrder = new OrderViewModel
            {
                Id = orderToUpdate.Id,
                Id_Product = product.Id,
                Id_User = user.Id,
                Amount = 555,
                Price = -0.0m,
                CompletionDate = DateTime.Now,
                Finished = false
            };

            // Act
            var updateResponse = await httpClient.PutAsJsonAsync(apiBaseUrlOrder, updatedOrder);

            // Assert
            Assert.AreEqual(HttpStatusCode.Conflict, updateResponse.StatusCode);          
        }
        [Test]
        public async Task DeleteOrder_WhenOrderExists_ShouldDeleteOrder()
        {
            // Arrange
            var httpClient = new HttpClient();

            var response = await httpClient.GetAsync(apiBaseUrlOrder);
            response.EnsureSuccessStatusCode();
            var orders = await response.Content.ReadFromJsonAsync<List<OrderViewModel>>();
            var orderToDelete = orders.Last(); 

            // Act
            var deleteResponse = await httpClient.DeleteAsync($"{apiBaseUrlOrder}/{orderToDelete.Id}");
            deleteResponse.EnsureSuccessStatusCode();
            var getResponse = await httpClient.GetAsync($"{apiBaseUrlOrder}/{orderToDelete.Id}");

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, getResponse.StatusCode);
        }
        [Test]
        public async Task DeleteOrder_WhenOrderDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            var httpClient = new HttpClient();
            var nonExistentClientId = "non_existent_client_id";

            // Act
            var deleteResponse = await httpClient.DeleteAsync($"{apiBaseUrlOrder}/{nonExistentClientId}");

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, deleteResponse.StatusCode);
        }
    }
}