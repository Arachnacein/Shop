using ClientManager.Dtos;
using ClientManager.Exceptions;
using ClientManager.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClientManager.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try{
                System.Console.WriteLine("-------- Getting --------");
                    var clients = _clientService.GetAllClients();
                    return Ok(clients);
            }
            catch(ClientNotFoundException e)
            {
                return NotFound(e);
            }
            catch(Exception e)
            {
                return Conflict(e);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            try{
                System.Console.WriteLine("-------- Getting by id --------");
                    var client = _clientService.GetClient(id);
                    return Ok(client);
            }
            catch(ClientNotFoundException e)
            {
                return NotFound(e);
            }
            catch(Exception e)
            {
                return Conflict(e);
            }
        }

        [HttpPost]
        public IActionResult Create(CreateClientDto newClient)
        {
            try{
                System.Console.WriteLine("-------- Creting --------");
                var client = _clientService.AddNewClient(newClient);
                return Created($"api/clients/{client.Id}", client);
            }
            catch(Exception e)
            {
                return Conflict(e.Message);
            }
        }

    }
}