using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;


namespace ClientManagerTests
{    
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class ClientTests : PageTest
    {
        [Test]
        public async Task LoadClientList()
        {
            // Navigate to the GUI page that displays the client list
            await Page.GotoAsync("http://localhost:8080/clients");  // Zaktualizuj port zgodnie z rzeczywistością

            // Wait for the client list to be visible
            await Page.WaitForSelectorAsync("div[data-testid='client-list']");

            // Check that the client list contains items
            var clientItems = await Page.QuerySelectorAllAsync("div[data-testid='client-item']");
            Assert.IsTrue(clientItems.Count > 0, "Client list should contain items.");

            // Optionally, you can check if specific clients are present in the list
            // For example, check if a client with a specific name is present
            var specificClient = await Page.QuerySelectorAsync("div[data-testid='client-item']:has-text('Jan Kowalski')");
            Assert.IsNotNull(specificClient, "Client list should contain 'Jan Kowalski'.");
        }
    }
}