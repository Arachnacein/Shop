using System;
using TechTalk.SpecFlow;
using NUnit.Framework;
using OrderManager.Data;
using OrderManager.Models;
using Microsoft.EntityFrameworkCore;

[Binding]
public class EditOrderSteps
{
    private int orderId = 1;
    private int updatedAmount;
    private decimal updatedPrice;
    private bool updatedFinished;

    private OrderDbContext order_context;

    [Given(@"istnieje zamówienie o id ""(.*)""")]
    public void GivenIstniejeZamowienieOId(int id)
    {
        order_context = GetInMemoryDbContextOrder();

        // Tworzymy zamówienie w bazie danych, które będziemy edytować
        var order = new Order
        {
            Id = orderId,
            Id_Product = 1,
            Id_User = Guid.Parse("00000000-0000-0000-0000-000000000001"),
            Amount = 2,
            Price = 4999.98m,
            CreateDate = DateTime.Now,
            CompletionDate = DateTime.Now.AddDays(7),
            Finished = false
        };
        
        order_context.Orders.Add(order);
        order_context.SaveChanges();

        // Upewniamy się, że zamówienie zostało dodane
        var existingOrder = order_context.Orders.FirstOrDefault(o => o.Id == id);
        Assert.IsNotNull(existingOrder, $"Zamówienie o Id {id} nie istnieje w bazie danych");
        Console.WriteLine($"Zamówienie o Id {id} istnieje w bazie danych");
    }

    [When(@"użytkownik edytuje zamówienie z Id ""(.*)"" zmieniając ilość na ""(.*)"" i cenę na ""(.*)""")]
    public void WhenUzytkownikEdytujeZamowienieZIdZmieniajacIloscNaICeneNa(int id, int amount, decimal price)
    {
        // Pobieramy zamówienie do edycji
        var order = order_context.Orders.FirstOrDefault(o => o.Id == id);
        Assert.IsNotNull(order, $"Zamówienie o Id {id} nie istnieje w bazie danych");

        // Aktualizujemy zamówienie
        order.Amount = amount;
        order.Price = price;
        updatedAmount = amount;
        updatedPrice = price;
        updatedFinished = order.Finished;

        order_context.Orders.Update(order);
        order_context.SaveChanges();

        Console.WriteLine($"Zamówienie o Id {id} zostało zaktualizowane: Ilość={amount}, Cena={price}");
    }

    [Then(@"zamówienie o Id ""(.*)"" ma ilość ""(.*)""")]
    public void ThenZamowienieOIlość(int id, int expectedAmount)
    {
        var order = order_context.Orders.FirstOrDefault(o => o.Id == id);
        Assert.IsNotNull(order, $"Zamówienie o Id {id} nie istnieje w bazie danych");
        Assert.AreEqual(expectedAmount, order.Amount, $"Oczekiwana ilość to {expectedAmount}, ale zamówienie ma {order.Amount}");
        Console.WriteLine($"Zamówienie o Id {id} ma ilość {order.Amount}");
    }

    [Then(@"zamówienie o Id ""(.*)"" ma cenę ""(.*)""")]
    public void ThenZamowienieOIdMaCenę(int id, decimal expectedPrice)
    {
        var order = order_context.Orders.FirstOrDefault(o => o.Id == id);
        Assert.IsNotNull(order, $"Zamówienie o Id {id} nie istnieje w bazie danych");
        Assert.AreEqual(expectedPrice, order.Price, $"Oczekiwana cena to {expectedPrice}, ale zamówienie ma {order.Price}");
        Console.WriteLine($"Zamówienie o Id {id} ma cenę {order.Price}");
    }

    [Then(@"zamówienie o Id ""(.*)"" ma status ""(.*)""")]
    public void ThenZamowienieOIdMaStatus(int id, string expectedStatus)
    {
        var order = order_context.Orders.FirstOrDefault(o => o.Id == id);
        Assert.IsNotNull(order, $"Zamówienie o Id {id} nie istnieje w bazie danych");

        var status = order.Finished ? "ukończone" : "nieukończone";
        Assert.AreEqual(expectedStatus, status, $"Oczekiwany status to {expectedStatus}, ale zamówienie ma status {status}");
        Console.WriteLine($"Zamówienie o Id {id} ma status {status}");
    }

    private OrderDbContext GetInMemoryDbContextOrder()
    {
        var options = new DbContextOptionsBuilder<OrderDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new OrderDbContext(options);
    }
}
