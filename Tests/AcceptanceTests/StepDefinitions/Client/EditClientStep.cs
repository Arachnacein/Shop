using System;
using TechTalk.SpecFlow;
using NUnit.Framework;
using ClientManager.Data;
using ClientManager.Models;
using Microsoft.EntityFrameworkCore;

[Binding]
public class EditClientSteps
{
    private Client _existingClient;
    private string _newClientName;
    private string _newClientSurname;
    private ClientDbContext _context;

    [Given(@"klient istnieje w systemie")]
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

    [When(@"użytkownik zmienia imię klienta na ""(.*)"" i nazwisko na ""(.*)""")]
    public void WhenUzytkownikZmieniaImieKlientaNaINazwiskoNa(string newName, string newSurname)
    {
        _newClientName = newName;
        _newClientSurname = newSurname;

        var clientToEdit = _context.Clients.Find(_existingClient.Id);
        if (clientToEdit != null)
        {
            clientToEdit.Name = _newClientName;
            clientToEdit.Surname = _newClientSurname;
            _context.SaveChanges();

            Console.WriteLine($"Dane klienta zostały zaktualizowane na: {clientToEdit.Name} {clientToEdit.Surname}");
        }
        else
        {
            Assert.Fail("Nie znaleziono klienta do edycji.");
        }
    }

    [When(@"zapisuje zmiany")]
    public void WhenZapisujeZmiany()
    {
        Console.WriteLine("Zmiany zostały zapisane.");
    }

    [Then(@"dane klienta są zaktualizowane w bazie danych")]
    public void ThenDaneKlientaSaZaktualizowaneWBazieDanych()
    {
        var updatedClient = _context.Clients.Find(_existingClient.Id);

        Assert.IsNotNull(updatedClient, "Nie znaleziono klienta po aktualizacji.");
        Assert.AreEqual(_newClientName, updatedClient.Name, "Imię klienta nie zostało zaktualizowane.");
        Assert.AreEqual(_newClientSurname, updatedClient.Surname, "Nazwisko klienta nie zostało zaktualizowane.");

        Console.WriteLine($"Dane klienta zostały pomyślnie zaktualizowane: {updatedClient.Name} {updatedClient.Surname}");
    }

    private ClientDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<ClientDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new ClientDbContext(options);
    }
}
