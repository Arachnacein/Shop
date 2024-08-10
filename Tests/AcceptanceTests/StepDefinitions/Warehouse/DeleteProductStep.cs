using System;
using TechTalk.SpecFlow;
using NUnit.Framework;
using WarehouseManager.Data;
using WarehouseManager.Models;
using Microsoft.EntityFrameworkCore;

[Binding]
public class DeleteProductSteps
{
    private string productName = "Laptop";
    private WarehouseDbContext context;

    [Given(@"użytkownik jest na stronie usuwania produktu")]
    public void GivenUzytkownikJestNaStronieUsuwaniaProduktu()
    {
        Console.WriteLine("Użytkownik jest na stronie usuwania produktu");
        context = GetInMemoryDbContext();
        
        var product = new Product
        {
            Name = productName,
            Price = 2499.99m,
            Amount = 10,
            UnitType = UnitEnum.piece,
            ProductType = ProductTypeEnum.Solid
        };
        context.Products.Add(product);
        context.SaveChanges();
    }

    [Given(@"istnieje produkt o nazwie ""(.*)""")]
    public void GivenIstniejeProduktONazwie(string name)
    {
        var product = context.Products.FirstOrDefault(p => p.Name == name);
        Assert.IsNotNull(product, $"Produkt o nazwie {name} nie istnieje w bazie danych");
        Console.WriteLine($"Produkt o nazwie {name} istnieje w bazie danych");
    }

    [When(@"użytkownik zatwierdza usunięcie produktu")]
    public void WhenUzytkownikZatwierdzaUsuniecieProduktu()
    {
        var product = context.Products.FirstOrDefault(p => p.Name == productName);
        if (product != null)
        {
            context.Products.Remove(product);
            context.SaveChanges();
            Console.WriteLine("Użytkownik zatwierdził usunięcie produktu");
        }
    }

    [Then(@"produkt jest usunięty z bazy danych")]
    public void ThenProduktJestUsunietyZBazyDanych()
    {
        var deletedProduct = context.Products.FirstOrDefault(p => p.Name == productName);
        Assert.IsNull(deletedProduct, "Produkt nie został usunięty z bazy danych");
        Console.WriteLine("Produkt został usunięty z bazy danych");
    }

    [Then(@"produkt nie jest już dostępny w systemie")]
    public void ThenProduktNieJestJuzDostepnyWSystemie()
    {
        var productExists = context.Products.Any(p => p.Name == productName);
        Assert.IsFalse(productExists, "Produkt nadal istnieje w systemie, mimo że powinien zostać usunięty");
        Console.WriteLine("Produkt nie jest już dostępny w systemie");
    }

    private WarehouseDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<WarehouseDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new WarehouseDbContext(options);
    }
}
