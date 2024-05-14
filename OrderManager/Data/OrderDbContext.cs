using OrderManager.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace OrderManager.Data
{
    public class OrderDbContext : DbContext
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> opt) : base(opt)
        {
            try{
                var dbCreate = Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
                if(dbCreate != null)
                {
                    if(!dbCreate.CanConnect())
                        dbCreate.Create();
                    if(!dbCreate.HasTables())
                        dbCreate.CreateTables();
                }
            }
            catch(Exception e)
            {
                System.Console.WriteLine($"Error {e.Message}");
            }
        }
        public DbSet<Order> Orders {get; set;}
    }
}