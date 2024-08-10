using System;
using TechTalk.SpecFlow;
using NUnit.Framework;
using ClientManager.Data;
using ClientManager.Models;
using Microsoft.EntityFrameworkCore;

[Binding]
public class DeleteClientSteps
{
    private Client _existingClient;
    private ClientDbContext _context;

    [Given(@"klient do usunięcia istnieje w systemie")]
    public void GivenKlientIstniejeWSystemie()
    {
        _context = GetInMemoryDbContext();

        _existingClient = new Client
        {
            Name = "Jan",
            Surname = "Kowalski"
        };

        _context.Clients.Add(_existingClient);
        _context.SaveChanges();

        Console.WriteLine($"Klient został dodany do systemu: {_existingClient.Name} {_existingClient.Surname}");
    }

    [When(@"użytkownik usuwa klienta")]
    public void WhenUzytkownikUsuwaKlienta()
    {
        var clientToDelete = _context.Clients.Find(_existingClient.Id);

        if (clientToDelete != null)
        {
            _context.Clients.Remove(clientToDelete);
            _context.SaveChanges();

            Console.WriteLine("Klient został usunięty z systemu.");
        }
        else
        {
            Assert.Fail("Nie znaleziono klienta do usunięcia.");
        }
    }

    [Then(@"klient jest usunięty z bazy danych")]
    public void ThenKlientJestUsunietyZBazyDanych()
    {
        var deletedClient = _context.Clients.Find(_existingClient.Id);

        Assert.IsNull(deletedClient, "Klient nie został usunięty z bazy danych.");
        Console.WriteLine("Klient został pomyślnie usunięty z bazy danych.");
    }

    private ClientDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<ClientDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new ClientDbContext(options);
    }
}
