using AutoMapper;
using ClientManager.Data.Repositories;
using ClientManager.Dtos;
using ClientManager.Exceptions;
using ClientManager.Mappers;
using ClientManager.Models;
using Microsoft.IdentityModel.Tokens;

namespace ClientManager.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IClientMapper _clientMapper;
        private readonly IMapper _mapper;

        public ClientService(IClientRepository clientRepository, IClientMapper ClientMapper, IMapper mapper)
        {
            _clientRepository = clientRepository;
            _clientMapper = ClientMapper;
            _mapper = mapper;
        }

        public ClientDto GetClient(Guid id)
        {
            var client = _clientRepository.GetClient(id);
            if(client == null)
                throw new ClientNotFoundException($"Client not found. Id:{id}");
            return _clientMapper.Map(client);
        }
        public IEnumerable<ClientDto> GetAllClients()
        {
            var clients = _clientRepository.GetClients();
            if(clients.IsNullOrEmpty())
                throw new ArgumentNullException($"Not found any clients!");
            return _clientMapper.MapElements(clients.ToList());
        }
        public ClientDto AddNewClient(CreateClientDto client)
        {
            var mappedClient = _mapper.Map<Client>(client);
            _clientRepository.AddClient(mappedClient);
            return _clientMapper.Map(mappedClient);
        }
        public void UpdateClient(ClientDto client)
        {
            var existingClient = _clientRepository.GetClient(client.Id);
            if(existingClient == null)
                throw new ClientNotFoundException($"Client not found. Id:{client.Id}");
            _clientRepository.UpdateClient(_clientMapper.Map(client));
        }
        public void DeleteClient(ClientDto client)
        {
            var existingClient = _clientRepository.GetClient(client.Id);
            if(existingClient == null)
                throw new ClientNotFoundException($"Client not found. Id:{client.Id}");
            _clientRepository.DeleteClient(client.Id);
        }
        public int CountClients() => _clientRepository.GetClients().Count();
        
    }
}