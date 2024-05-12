using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using WarehouseManager.Models;

namespace WarehouseManager.Data
{
    public class WarehouseDbContext : DbContext
    {
        public WarehouseDbContext(DbContextOptions<WarehouseDbContext> options) : base(options)
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

        public DbSet<Product> Products {get;set;}
    }
}