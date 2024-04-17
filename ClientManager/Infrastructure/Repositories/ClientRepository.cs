using ClientManager.Data;
using ClientManager.Data.Repositories;
using ClientManager.Models;

namespace ClientManager.Infrastructure.Repositories
{

    public class ClientRepository : IClientRepository
    {
        private readonly ClientDbContext _context;
        public ClientRepository(ClientDbContext context)
        {
            _context = context;
        }

        public bool SaveChanges()
        {
            return _context.SaveChanges() >= 0;
        }

        public IEnumerable<Client> GetClients()
        {
            return _context.Clients;
        }

        public Client GetClient(Guid id)
        {
            return _context.Clients.Single(x => x.Id == id);
        }

        public Client GetClient(string name)
        {
            return _context.Clients.Single(x => x.Name == name);
        }

        public Client AddClient(Client client)
        {
            if(client == null)
                throw new ArgumentNullException(nameof(client));

            client.RegistryDate = DateTime.Now;
            _context.Clients.Add(client);
            _context.SaveChanges();
            return client;
        }

        public void DeleteClient(Guid id)
        {
            var client = _context.Clients.Single(x => x.Id == id);
            _context.Clients.Remove(client);
            _context.SaveChanges(); 
        }

        public void UpdateClient(Client client)
        {
            _context.Clients.Update(client);
            _context.SaveChanges();
        }
    }
}