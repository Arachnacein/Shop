using ClientManager.Models;

namespace ClientManager.Data.Repositories
{
    public interface IClientRepository
    {
        bool SaveChanges();
        IEnumerable<Client> GetClients();
        Client GetClient(Guid id);
        Client GetClient(string name);
        Client AddClient(Client client);
        void UpdateClient(Client client);
        void DeleteClient(Guid id);
    }

}