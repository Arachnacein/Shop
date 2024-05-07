using ClientManager.Dtos;

namespace ClientManager.Services
{
    public interface IClientService
    {
        IEnumerable<ClientDto> GetAllClients();
        ClientDto GetClient(Guid id);
        ClientDto AddNewClient(CreateClientDto client);
        void UpdateClient(UpdateClientDto client);
        void DeleteClient(Guid id);
        int CountClients();
    }
}