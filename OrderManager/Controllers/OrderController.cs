using Microsoft.AspNetCore.Mvc;
using OrderManager.Dto;
using OrderManager.Exceptions;
using OrderManager.Services;


namespace OrderManager.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService service)
    {
        _orderService = service;
    }

    [HttpGet]
    public IActionResult Get()
    {
        try{
            var orders = _orderService.GetAllOrders();
            return Ok(orders);
        }
        catch(OrderNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch(Exception e)
        {
            return Conflict(e.Message);
        }
    }

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        try{
            var order = _orderService.GetOrderById(id);
            return Ok(order);
        }
        catch(OrderNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch(Exception e)
        {
            return Conflict(e.Message);
        }
    }

    [HttpPost]
    public IActionResult Create(CreateOrderDto dto)
    {
        try{
            var order = _orderService.CreateOrder(dto);
            return Created($"api/orders/{order.Id}", order);
        }
        catch(OrderNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch(Exception e)
        {
            return Conflict(e.Message);
        }
    }

    [HttpPut]
    public IActionResult Update(UpdateOrderDto dto)
    {
        try{
            _orderService.UpdateOrder(dto);
            return NoContent();
        }
        catch(OrderNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch(Exception e)
        {
            return Conflict(e.Message);
        }
    }

    [HttpPut("FinishOrder/{id}")]
    public IActionResult FinishOrder(int id)
    {
        try{
            _orderService.FinishOrder(id);
            return NoContent();
        }
        catch(OrderNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch(Exception e)
        {
            return Conflict(e.Message);
        } 
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        try{
            _orderService.DeleteOrder(id);
            return NoContent();
        }
        catch(OrderNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch(Exception e)
        {
            return Conflict(e.Message);
        }        
    }
}
