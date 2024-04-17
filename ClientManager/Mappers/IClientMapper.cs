using ClientManager.Dtos;
using ClientManager.Models;

namespace ClientManager.Mappers
{
    public interface IClientMapper
    {
        ClientDto Map (Client source);
        Client Map (ClientDto source);
        ICollection<Client> MapElements (ICollection<ClientDto> sources);
        ICollection<ClientDto> MapElements (ICollection<Client> sources);
    }
}