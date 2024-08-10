using System;
using TechTalk.SpecFlow;
using NUnit.Framework;
using OrderManager.Data;
using OrderManager.Models;
using Microsoft.EntityFrameworkCore;

[Binding]
public class CompleteOrderSteps
{
    private int orderId = 1;
    private OrderDbContext order_context;

    [Given(@"istnieje zamówienie o Id ""(.*)"" z nieukończonym statusem")]
    public void GivenIstniejeZamowienieOIdZNieukonczonymStatusem(int id)
    {
        order_context = GetInMemoryDbContextOrder();

        // Tworzymy zamówienie w bazie danych z nieukończonym statusem
        var order = new Order
        {
            Id = orderId,
            Id_Product = 1,
            Id_User = Guid.Parse("00000000-0000-0000-0000-000000000001"),
            Amount = 2,
            Price = 4999.98m,
            CreateDate = DateTime.Now.AddDays(-7),
            CompletionDate = DateTime.Now,
            Finished = false
        };
        
        order_context.Orders.Add(order);
        order_context.SaveChanges();

        // Upewniamy się, że zamówienie zostało dodane i ma status nieukończony
        var existingOrder = order_context.Orders.FirstOrDefault(o => o.Id == id && !o.Finished);
        Assert.IsNotNull(existingOrder, $"Zamówienie o Id {id} nie istnieje w bazie danych lub jest już ukończone");
        Console.WriteLine($"Zamówienie o Id {id} istnieje w bazie danych i ma status nieukończony");
    }

    [When(@"użytkownik oznacza zamówienie o Id ""(.*)"" jako ukończone")]
    public void WhenUzytkownikOznaczaZamowienieOIdJakoUkonczone(int id)
    {
        var order = order_context.Orders.FirstOrDefault(o => o.Id == id);
        Assert.IsNotNull(order, $"Zamówienie o Id {id} nie istnieje w bazie danych");

        // Oznaczamy zamówienie jako ukończone
        order.Finished = true;
        order_context.SaveChanges();

        Console.WriteLine($"Zamówienie o Id {id} zostało oznaczone jako ukończone");
    }

    [Then(@"zamówienie o Id ""(.*)"" ma status ukończone")]
    public void ThenZamowienieOIdMaStatusUkonczone(int id)
    {
        var order = order_context.Orders.FirstOrDefault(o => o.Id == id);
        Assert.IsNotNull(order, $"Zamówienie o Id {id} nie zostało znalezione w bazie danych");
        Assert.IsTrue(order.Finished, "Zamówienie nie ma statusu ukończonego");
        Console.WriteLine($"Zamówienie o Id {id} ma status ukończone");
    }

    private OrderDbContext GetInMemoryDbContextOrder()
    {
        var options = new DbContextOptionsBuilder<OrderDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new OrderDbContext(options);
    }
}
