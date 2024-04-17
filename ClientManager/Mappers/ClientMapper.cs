using ClientManager.Dtos;
using ClientManager.Models;

namespace ClientManager.Mappers
{
    public class ClientMapper : IClientMapper
    {
        public ClientDto Map(Client source)
        {
            ClientDto destination = new ClientDto();
            destination.Id = source.Id;
            destination.Name = source.Name;
            destination.Surname = source.Surname;
            destination.RegistryDate = source.RegistryDate;
            return destination;
        }
        public ICollection<ClientDto> MapElements(ICollection<Client> sources)
        {
            List<ClientDto> destination = new List<ClientDto>();
            foreach(var item in sources)
                destination.Add(Map(item));

            return destination;
        }

        public Client Map(ClientDto source)
        {
            Client destination = new Client();
            destination.Id = source.Id;
            destination.Name = source.Name;
            destination.Surname = source.Surname;
            destination.RegistryDate = source.RegistryDate;
            return destination;
        }

        public ICollection<Client> MapElements(ICollection<ClientDto> sources)
        {
            List<Client> destination = new List<Client>();
            foreach(var item in sources)
                destination.Add(Map(item));

            return destination;
        }
    }
}