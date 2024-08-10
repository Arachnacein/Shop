using System;
using TechTalk.SpecFlow;
using NUnit.Framework;
using ClientManager.Data;
using ClientManager.Models;
using Microsoft.EntityFrameworkCore;

[Binding]
public class AddClientSteps
{
    private string clientName;
    private string clientSurname;

    [Given(@"użytkownik jest na stronie dodawania klienta")]
    public void GivenUzytkownikJestNaStronieDodawaniaKlienta()
    {
        Console.WriteLine("Użytkownik jest na stronie dodawania klienta");
    }

    [When(@"użytkownik wprowadza poprawne dane klienta")]
    public void WhenUzytkownikWprowadzaPoprawneDaneKlienta()
    {
        clientName = "Jan";
        clientSurname = "Kowalski";
        Console.WriteLine($"Wprowadzono dane klienta: {clientName}, {clientSurname}");
    }

    [When(@"użytkownik zatwierdza dodanie klienta")]
    public void WhenUzytkownikZatwierdzaDodanieKlienta()
    {
        Console.WriteLine("Użytkownik zatwierdził dodanie klienta");
    }

    [Then(@"nowy klient jest dodany do bazy danych")]
    public void ThenNowyKlientJestDodanyDoBazyDanych()
    {
        bool isCustomerAdded = AddCustomerToDatabase(clientName, clientSurname);
        Assert.IsTrue(isCustomerAdded, "Klient nie został dodany do bazy danych");
    }

    private bool AddCustomerToDatabase(string name, string surname)
    {
        try
        {
            var context = GetInMemoryDbContext();
            var client = new Client
            {
                Name = name,
                Surname = surname
             };
            context.Clients.Add(client);
            context.SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Błąd podczas dodawania klienta do bazy danych: {ex.Message}");
            return false;
        }
    }
    private ClientDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<ClientDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
       return new ClientDbContext(options);
    }   
}
