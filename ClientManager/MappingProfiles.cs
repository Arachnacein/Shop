using AutoMapper;
using ClientManager.Dtos;
using ClientManager.Models;

namespace ClientManager
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<CreateClientDto, Client>();
        }
    }
}