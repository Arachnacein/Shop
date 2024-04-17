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

    }
}