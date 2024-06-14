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
using MudBlazor;
using MudBlazor.Services;
using Bunit;

namespace UITests
{    
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class WarehousePageTests : PageTest
    {
        private readonly string apiBaseUrl = "http://localhost:8010/api/product";

        private async Task Add_Product()
        {
            var newProduct = new 
            {
                Name = "temporary product for tests",
                Price = 100.50m,
                Amount = 10150,
                UnitType = "Kilogram", 
                ProductType = "Ciekły" 
            };
        }
        [Test]
        public async Task AddProduct_WithValidData_ShouldAddProduct()
        {
            // Arrange
            var httpClient = new HttpClient();
            var newProduct = new 
            {
                Name = "Lays [salty]",
                Price = 10.50m,
                Amount = 50,
                UnitType = "Kilogram", 
                ProductType = "Ciekły" 
            };

            // Act
            var response = await httpClient.PostAsJsonAsync(apiBaseUrl, newProduct);
            var getProductsList = await httpClient.GetAsync(apiBaseUrl);
            var freshProducts = await getProductsList.Content.ReadFromJsonAsync<List<ProductViewModel>>();
            var freshProduct = freshProducts.Last();

            // // Assert
            Assert.IsNotNull(newProduct);
            Assert.AreEqual(newProduct.Name, freshProduct.Name);
            Assert.AreEqual(newProduct.Price, freshProduct.Price);
        }
        [Test]
        public async Task AddProduct_WithInvalidName_ShouldNotAddProduct()
        {
            // Arrange
            var httpClient = new HttpClient();
            var newProduct = new 
            {
                Name = string.Empty,
                Price = 10.50m,
                Amount = 50,
                UnitType = "Kilogram", 
                ProductType = "Ciekły" 
            };

            // Act
            var response = await httpClient.PostAsJsonAsync(apiBaseUrl, newProduct);

            // // Assert
            Assert.AreEqual(HttpStatusCode.Conflict, response.StatusCode);

        }
        [Test]
        public async Task AddProduct_WithInvalidPrice_ShouldNotAddProduct()
        {
            // Arrange
            var httpClient = new HttpClient();
            var newProduct = new 
            {
                Name = "CorretName",
                Price = -10.50m,
                Amount = 50,
                UnitType = "Kilogram", 
                ProductType = "Ciekły" 
            };

            // Act
            var response = await httpClient.PostAsJsonAsync(apiBaseUrl, newProduct);

            // // Assert
            Assert.AreEqual(HttpStatusCode.Conflict, response.StatusCode);

        }
        [Test]
        public async Task AddProduct_WithInvalidAmount_ShouldNotAddProduct()
        {
            // Arrange
            var httpClient = new HttpClient();
            var newProduct = new 
            {
                Name = "CorretName",
                Price = 10.50m,
                Amount = -50,
                UnitType = "Kilogram", 
                ProductType = "Ciekły" 
            };

            // Act
            var response = await httpClient.PostAsJsonAsync(apiBaseUrl, newProduct);

            // // Assert
            Assert.AreEqual(HttpStatusCode.Conflict, response.StatusCode);

        }
        [Test]
        public async Task AddProduct_WithInvalidUnitType_ShouldNotAddProduct()
        {
            // Arrange
            var httpClient = new HttpClient();
            var newProduct = new 
            {
                Name = "CorretName",
                Price = 10.50m,
                Amount = 50,
                UnitType = "BadUnitType", 
                ProductType = "Ciekły" 
            };

            // Act
            var response = await httpClient.PostAsJsonAsync(apiBaseUrl, newProduct);

            // // Assert
            Assert.AreEqual(HttpStatusCode.Conflict, response.StatusCode);

        }
        [Test]
        public async Task AddProduct_WithInvalidProductType_ShouldNotAddProduct()
        {
            // Arrange
            var httpClient = new HttpClient();
            var newProduct = new 
            {
                Name = "CorretName",
                Price = 10.50m,
                Amount = -50,
                UnitType = "Kilogram", 
                ProductType = "BadProductType" 
            };

            // Act
            var response = await httpClient.PostAsJsonAsync(apiBaseUrl, newProduct);

            // // Assert
            Assert.AreEqual(HttpStatusCode.Conflict, response.StatusCode);

        }
        [Test]
        public async Task GetProductList_WhenCalled_ShouldReturnNonEmptyListOfProducts()
        {
            // Arrange
            var httpClient = new HttpClient();

            // Act
            var response = await httpClient.GetAsync(apiBaseUrl);
            response.EnsureSuccessStatusCode();

            var products = await response.Content.ReadFromJsonAsync<List<ProductViewModel>>();

            // Assert
            Assert.IsNotNull(products);
            Assert.IsTrue(products.Count > 0, "Product list should not be empty.");
        }

        [Test]
        public async Task UpdateProduct_WhenDataIsValid_ShouldUpdateProductInformation()
        {
            // Arrange
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(apiBaseUrl);
            var products = await response.Content.ReadFromJsonAsync<List<ProductViewModel>>();
           
            var productToUpdate = products.Last(); 
            var updatedProduct = new 
            {
                Id = productToUpdate.Id,
                Name = "Lays [salt]",
                Price = 10.50m,
                Amount = 50,
                UnitType = "Kilogram", 
                ProductType = "Stały" 
            };

            // Act
            var updateResponse = await httpClient.PutAsJsonAsync(apiBaseUrl, updatedProduct);
            var getUpdatedResponse = await httpClient.GetAsync($"{apiBaseUrl}/{productToUpdate.Id}");
            var updatedProductResponse = await getUpdatedResponse.Content.ReadFromJsonAsync<ProductViewModel>();

            // Assert
            Assert.IsNotNull(updatedProductResponse);
            Assert.AreEqual(updatedProduct.Name, updatedProductResponse.Name);
            Assert.AreEqual(updatedProduct.Price, updatedProductResponse.Price);
        }

        [Test]
        public async Task DeleteProduct_WhenProductExists_ShouldDeleteProduct()
        {
            // Arrange
            Add_Product();
            var httpClient = new HttpClient();

            var response = await httpClient.GetAsync(apiBaseUrl);
            response.EnsureSuccessStatusCode();
            var products = await response.Content.ReadFromJsonAsync<List<ProductViewModel>>();
            var productToDelete = products.Last(); 

            // Act
            var deleteResponse = await httpClient.DeleteAsync($"{apiBaseUrl}/{productToDelete.Id}");
            deleteResponse.EnsureSuccessStatusCode();
            var getResponse = await httpClient.GetAsync($"{apiBaseUrl}/{productToDelete.Id}");

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, getResponse.StatusCode);
        }

        [Test]
        public async Task DeleteProduct_WhenProductDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            var httpClient = new HttpClient();
            var nonExistentProductId = 0; // Id produktu, który nie istnieje

            // Act
            var deleteResponse = await httpClient.DeleteAsync($"{apiBaseUrl}/{nonExistentProductId}");

            // Assert
            Assert.AreEqual(HttpStatusCode.Conflict, deleteResponse.StatusCode);
        }
    }
}