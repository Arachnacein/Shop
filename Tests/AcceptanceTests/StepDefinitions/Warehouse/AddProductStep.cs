using System;
using TechTalk.SpecFlow;
using NUnit.Framework;
using WarehouseManager.Data;
using WarehouseManager.Models;
using Microsoft.EntityFrameworkCore;

[Binding]
public class AddProductSteps
{
    private string productName;
    private decimal productPrice;
    private int productAmount;
    private UnitEnum unitType;
    private ProductTypeEnum productType;

    [Given(@"użytkownik jest na stronie dodawania produktu")]
    public void GivenUzytkownikJestNaStronieDodawaniaProduktu()
    {
        Console.WriteLine("Użytkownik jest na stronie dodawania produktu");
    }

    [When(@"użytkownik wprowadza poprawne dane produktu")]
    public void WhenUzytkownikWprowadzaPoprawneDaneProduktu()
    {
        productName = "Laptop";
        productPrice = 2499.99m;
        productAmount = 10;
        unitType = UnitEnum.piece;
        productType = ProductTypeEnum.Solid;

        Console.WriteLine(
            $"Wprowadzono dane produktu: {productName}, {productPrice}, {productAmount}, {unitType}, {productType}"
            );
    }

    [When(@"użytkownik zatwierdza dodanie produktu")]
    public void WhenUzytkownikZatwierdzaDodanieProduktu()
    {
        Console.WriteLine("Użytkownik zatwierdził dodanie produktu");
    }

    [Then(@"nowy produkt jest dodany do bazy danych")]
    public void ThenNowyProduktJestDodanyDoBazyDanych()
    {
        bool isProductAdded = AddProductToDatabase(productName, productPrice, productAmount, unitType, productType);
        Assert.IsTrue(isProductAdded, "Produkt nie został dodany do bazy danych");
    }

    private bool AddProductToDatabase(string name, decimal price, int amount, UnitEnum unitType, 
                                        ProductTypeEnum productType)
    {
        try
        {
            var context = GetInMemoryDbContext();
            var product = new Product
            {
                Name = name,
                Price = price,
                Amount = amount,
                UnitType = (UnitEnum)unitType,
                ProductType = (ProductTypeEnum)productType
            };
            context.Products.Add(product);
            context.SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Błąd podczas dodawania produktu do bazy danych: {ex.Message}");
            return false;
        }
    }

    private WarehouseDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<WarehouseDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new WarehouseDbContext(options);
    }
}
