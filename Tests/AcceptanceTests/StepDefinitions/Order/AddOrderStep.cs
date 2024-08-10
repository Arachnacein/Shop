using System;
using TechTalk.SpecFlow;
using NUnit.Framework;
using OrderManager.Data;
using ClientManager.Data;
using WarehouseManager.Data;
using OrderManager.Models;
using ClientManager.Models;
using WarehouseManager.Models;
using Microsoft.EntityFrameworkCore;

[Binding]
public class AddOrderSteps
{
    private int productId = 1;
    private Guid userId = Guid.Parse("00000000-0000-0000-0000-000000000001");
    private int orderAmount;
    private decimal orderPrice;
    private DateTime createDate;
    private DateTime completionDate;
    private bool finished;

    private OrderDbContext order_context;
    private ClientDbContext client_context;
    private WarehouseDbContext product_context;

    [Given(@"użytkownik jest na stronie dodawania zamówienia")]
    public void GivenUzytkownikJestNaStronieDodawaniaZamowienia()
    {
        Console.WriteLine("Użytkownik jest na stronie dodawania zamówienia");
        product_context = GetInMemoryDbContextWarehouse();
        client_context = GetInMemoryDbContextClient();

        product_context.Products.Add(new Product { Id = productId, Name = "Laptop", Price = 2499.99m, Amount = 10, UnitType = UnitEnum.piece, ProductType = ProductTypeEnum.Solid });
        client_context.Clients.Add(new Client { Id = userId, Name = "TestUser", Surname = "TestSurname"  });
        client_context.SaveChanges();
        product_context.SaveChanges();
    }

    [Given(@"istnieje produkt o Id ""(.*)""")]
    public void GivenIstniejeProduktOId(int id)
    {
        var product = product_context.Products.FirstOrDefault(p => p.Id == id);
        Assert.IsNotNull(product, $"Produkt o Id {id} nie istnieje w bazie danych");
        Console.WriteLine($"Produkt o Id {id} istnieje w bazie danych");
    }

    [Given(@"istnieje użytkownik o Id ""(.*)""")]
    public void GivenIstniejeUzytkownikOId(Guid id)
    {
        var client = client_context.Clients.FirstOrDefault(u => u.Id == id);
        Assert.IsNotNull(client, $"Użytkownik o Id {id} nie istnieje w bazie danych");
        Console.WriteLine($"Użytkownik o Id {id} istnieje w bazie danych");
    }

    [When(@"użytkownik wprowadza poprawne dane zamówienia")]
    public void WhenUzytkownikWprowadzaPoprawneDaneZamowienia()
    {
        orderAmount = 2;
        orderPrice = 4999.98m; 
        createDate = DateTime.Now;
        completionDate = DateTime.Now.AddDays(7);
        finished = false;

        Console.WriteLine($"Wprowadzono dane zamówienia: ProduktId={productId}, UserId={userId}, Ilość={orderAmount}, Cena={orderPrice}, Data utworzenia={createDate}, Data zakończenia={completionDate}, Ukończone={finished}");
    }

    [When(@"użytkownik zatwierdza dodanie zamówienia")]
    public void WhenUzytkownikZatwierdzaDodanieZamowienia()
    {
        var order = new Order
        {
            Id_Product = productId,
            Id_User = userId,
            Amount = orderAmount,
            Price = orderPrice,
            CreateDate = createDate,
            CompletionDate = completionDate,
            Finished = finished
        };
        
        order_context = GetInMemoryDbContextOrder();
        order_context.Orders.Add(order);
        order_context.SaveChanges();
        Console.WriteLine("Użytkownik zatwierdził dodanie zamówienia");
    }

    [Then(@"nowe zamówienie jest dodane do bazy danych")]
    public void ThenNoweZamowienieJestDodaneDoBazyDanych()
    {
        
        var order = order_context.Orders.FirstOrDefault(o => o.Id_Product == productId && o.Id_User == userId);
        Assert.IsNotNull(order, "Zamówienie nie zostało dodane do bazy danych");
        Console.WriteLine("Nowe zamówienie zostało dodane do bazy danych");
    }

    [Then(@"zamówienie ma status nieukończone")]
    public void ThenZamowienieMaStatusNieukonczone()
    {
        var order = order_context.Orders.FirstOrDefault(o => o.Id_Product == productId && o.Id_User == userId);
        Assert.IsNotNull(order, "Zamówienie nie zostało znalezione w bazie danych");
        Assert.IsFalse(order.Finished, "Zamówienie nie ma statusu nieukończonego");
        Console.WriteLine("Zamówienie ma status nieukończone");
    }

    private OrderDbContext GetInMemoryDbContextOrder()
    {
        var options = new DbContextOptionsBuilder<OrderDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new OrderDbContext(options);
    }
    private ClientDbContext GetInMemoryDbContextClient()
    {
        var options = new DbContextOptionsBuilder<ClientDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new ClientDbContext(options);
    }
    private WarehouseDbContext GetInMemoryDbContextWarehouse()
    {
        var options = new DbContextOptionsBuilder<WarehouseDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new WarehouseDbContext(options);
    }
}