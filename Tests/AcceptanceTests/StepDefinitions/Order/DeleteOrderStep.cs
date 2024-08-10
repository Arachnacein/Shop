using System;
using TechTalk.SpecFlow;
using NUnit.Framework;
using OrderManager.Data;
using OrderManager.Models;
using Microsoft.EntityFrameworkCore;

[Binding]
public class DeleteOrderSteps
{
    private int orderId = 1;
    private OrderDbContext order_context;

    [Given(@"istnieje zamówienie o Id ""(.*)""")]
    public void GivenIstniejeZamowienieOId(int id)
    {
        order_context = GetInMemoryDbContextOrder();

        // Tworzymy zamówienie w bazie danych, które będziemy usuwać
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

    [When(@"użytkownik usuwa zamówienie o Id ""(.*)""")]
    public void WhenUzytkownikUsuwaZamowienieOId(int id)
    {
        var order = order_context.Orders.FirstOrDefault(o => o.Id == id);
        Assert.IsNotNull(order, $"Zamówienie o Id {id} nie istnieje w bazie danych");

        // Usuwamy zamówienie
        order_context.Orders.Remove(order);
        order_context.SaveChanges();

        Console.WriteLine($"Zamówienie o Id {id} zostało usunięte z bazy danych");
    }

    [Then(@"zamówienie o Id ""(.*)"" nie istnieje w bazie danych")]
    public void ThenZamowienieOIdNieIstniejeWBazieDanych(int id)
    {
        var order = order_context.Orders.FirstOrDefault(o => o.Id == id);
        Assert.IsNull(order, $"Zamówienie o Id {id} nadal istnieje w bazie danych");
        Console.WriteLine($"Zamówienie o Id {id} nie istnieje w bazie danych");
    }

    private OrderDbContext GetInMemoryDbContextOrder()
    {
        var options = new DbContextOptionsBuilder<OrderDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new OrderDbContext(options);
    }
}
