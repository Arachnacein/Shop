using ClientManager.Dtos;

namespace ClientManager.Services
{
    public interface IClientService
    {
        IEnumerable<ClientDto> GetAllClients();
        ClientDto GetClient(Guid id);
        ClientDto AddNewClient(ClientDto client);
        void UpdateClient(ClientDto client);
        int CountClients();
        void DeleteClient(ClientDto client);
    }
}