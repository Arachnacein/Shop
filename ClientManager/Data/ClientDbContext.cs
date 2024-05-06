using ClientManager.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace ClientManager.Data
{
    public class ClientDbContext : DbContext
    {
        public ClientDbContext(DbContextOptions<ClientDbContext> options) : base(options)
        {
                try{
                    var dbCreator = Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
                    if(dbCreator != null)
                    {
                        if(!dbCreator.CanConnect())
                            dbCreator.Create();
                        if(!dbCreator.HasTables())
                            dbCreator.CreateTables();
                    }
                }
                catch(Exception e)
                {
                    System.Console.WriteLine($"Error: {e.Message} source: {e.Source}");
                }
        }

        public DbSet<Client> Clients {get;set;}
    }


}