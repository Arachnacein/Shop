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
            if(client.Name == null || client.Name == string.Empty || client.Name.Length < 2)
                throw new Exception($"Name should be at least 2 signs.");
            if(client.Surname == null || client.Surname == string.Empty || client.Surname.Length <4)
                throw new Exception($"Surname should be at least 4 signs.");
                
            var mappedClient = _mapper.Map<Client>(client);
            _clientRepository.AddClient(mappedClient);
            return _clientMapper.Map(mappedClient);
        }
        public void UpdateClient(UpdateClientDto client)
        {
            if(client.Name == null || client.Name == string.Empty || client.Name.Length < 2)
                throw new Exception($"Name should be at least 2 signs.");
            if(client.Surname == null || client.Surname == string.Empty || client.Surname.Length <4)
                throw new Exception($"Surname should be at least 4 signs.");

            var existingClient = _clientRepository.GetClient(client.Id);
            if(existingClient == null)
                throw new ClientNotFoundException($"Client not found. Id:{client.Id}");
            
            var result = _mapper.Map<Client>(client);
            _clientRepository.UpdateClient(result);
        }
        public void DeleteClient(Guid id)
        {
            var existingClient = _clientRepository.GetClient(id);
            if(existingClient == null)
                throw new ClientNotFoundException($"Client not found. Id:{id}");
            _clientRepository.DeleteClient(id);
        }
        public int CountClients() => _clientRepository.GetClients().Count();
        
    }
}