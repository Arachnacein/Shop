using System;
using TechTalk.SpecFlow;
using NUnit.Framework;
using WarehouseManager.Data;
using WarehouseManager.Models;
using Microsoft.EntityFrameworkCore;

[Binding]
public class EditProductSteps
{
    private string originalProductName = "Laptop";
    private string newProductName;
    private decimal newProductPrice;
    private int newProductAmount;
    private UnitEnum newUnitType;
    private ProductTypeEnum newProductType;

    private WarehouseDbContext context;
    private Product originalProduct;

    [Given(@"użytkownik jest na stronie edytowania produktu")]
    public void GivenUzytkownikJestNaStronieEdytowaniaProduktu()
    {
        Console.WriteLine("Użytkownik jest na stronie edytowania produktu");
        context = GetInMemoryDbContext();
        originalProduct = new Product
        {
            Name = originalProductName,
            Price = 2499.99m,
            Amount = 10,
            UnitType = UnitEnum.piece,
            ProductType = ProductTypeEnum.Solid
        };
        context.Products.Add(originalProduct);
        context.SaveChanges();
    }

    [Given(@"istnieje produkt nazwany ""(.*)""")]
    public void GivenIstniejeProduktONazwie(string productName)
    {
        Assert.IsNotNull(context.Products.FirstOrDefault(p => p.Name == productName), $"Produkt o nazwie {productName} nie istnieje w bazie danych");
        Console.WriteLine($"Produkt o nazwie {productName} istnieje w bazie danych");
    }

    [When(@"użytkownik wprowadza nowe dane produktu")]
    public void WhenUzytkownikWprowadzaNoweDaneProduktu()
    {
        newProductName = "Laptop Pro";
        newProductPrice = 2999.99m;
        newProductAmount = 5;
        newUnitType = UnitEnum.piece;
        newProductType = ProductTypeEnum.Solid;

        Console.WriteLine($"Wprowadzono nowe dane produktu: {newProductName}, {newProductPrice}, {newProductAmount}, {newUnitType}, {newProductType}");
    }

    [When(@"użytkownik zatwierdza edytowanie produktu")]
    public void WhenUzytkownikZatwierdzaEdytowanieProduktu()
    {
        var product = context.Products.FirstOrDefault(p => p.Name == originalProductName);
        if (product != null)
        {
            product.Name = newProductName;
            product.Price = newProductPrice;
            product.Amount = newProductAmount;
            product.UnitType = newUnitType;
            product.ProductType = newProductType;

            context.SaveChanges();
            Console.WriteLine("Użytkownik zatwierdził edytowanie produktu");
        }
    }

    [Then(@"zmienione dane produktu są zapisane w bazie danych")]
    public void ThenZmienioneDaneProduktuSaZapisaneWBazieDanych()
    {
        var updatedProduct = context.Products.FirstOrDefault(p => p.Name == newProductName);
        Assert.IsNotNull(updatedProduct, "Produkt nie został zaktualizowany w bazie danych");

        Assert.AreEqual(newProductPrice, updatedProduct.Price, "Cena produktu nie została prawidłowo zaktualizowana");
        Assert.AreEqual(newProductAmount, updatedProduct.Amount, "Ilość produktu nie została prawidłowo zaktualizowana");
        Assert.AreEqual(newUnitType, updatedProduct.UnitType, "Jednostka produktu nie została prawidłowo zaktualizowana");
        Assert.AreEqual(newProductType, updatedProduct.ProductType, "Typ produktu nie został prawidłowo zaktualizowany");

        Console.WriteLine("Zmienione dane produktu zostały zapisane w bazie danych");
    }

    private WarehouseDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<WarehouseDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new WarehouseDbContext(options);
    }
}
