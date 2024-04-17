using ClientManager.Models;
using Microsoft.EntityFrameworkCore;

namespace ClientManager.Data
{
    public class ClientDbContext : DbContext
    {
        public ClientDbContext(DbContextOptions<ClientDbContext> options) : base(options)
        {

        }

        public DbSet<Client> Clients {get;set;}
    }


}