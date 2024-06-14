using Microsoft.AspNetCore.Mvc;
using ClientManager.Dtos;
using ClientManager.Exceptions;
using ClientManager.Services;

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
                    var clients = _clientService.GetAllClients();
                    return Ok(clients);
            }
            catch(ClientNotFoundException e)
            {
                return NotFound("No clients found");
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
                    var client = _clientService.GetClient(id);
                    return Ok(client);
            }
            catch(ClientNotFoundException e)
            {
                return NotFound(e.Message);
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
                var client = _clientService.AddNewClient(newClient);
                return Created($"api/clients/{client.Id}", client);
            }
            catch(Exception e)
            {
                return Conflict(e.Message);
            }
        }

        [HttpPut]
        public IActionResult Update(UpdateClientDto updateClient)
        {
            try{
                _clientService.UpdateClient(updateClient);
                return NoContent();
            }
            catch(ClientNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch(Exception e)
            {
                return Conflict(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            try{
                _clientService.DeleteClient(id);
                return NoContent();
            }
            catch(ClientNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch(Exception e)
            {
                return Conflict(e);
            }
        }
    }
}