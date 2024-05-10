using Microsoft.AspNetCore.Mvc;
using WarehouseManager.Dtos;
using WarehouseManager.Exceptions;
using WarehouseManager.Services;

namespace WarehouseManager.Controllers;

[ApiController]
[Route("[controller]")]
public class WarehouseController : ControllerBase
{
    private readonly IWarehouseService _warehouseService;
    public WarehouseController(IWarehouseService warehouseService)
    {
        _warehouseService = warehouseService;
    }

    [HttpGet]
    public IActionResult Get()
    {
        try{
            var products = _warehouseService.GetAllProducts();
            return Ok(products);
        }
        catch(ProductNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch(Exception e)
        {
            return Conflict(e);
        }
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        try{
            var product = _warehouseService.GetProduct(id);
            return Ok(product);
        }
        catch(ProductNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch(Exception e)
        {
            return Conflict(e);
        } 
    }

    [HttpPost]
    public IActionResult Create(CreateProductDto dto)
    {
        try{
            var product = _warehouseService.AddNewProduct(dto);
            return Created($"apli/products/{product.Id}", product);
        }
        catch(Exception e)
        {
            return Conflict(e);
        } 
    }

    [HttpPut]
    public IActionResult Update(UpdateProductDto dto)
    {
        try{
            _warehouseService.UpdateProduct(dto);
            return NoContent();
        }
        catch(ProductNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch(Exception e)
        {
            return Conflict(e);
        } 
    }

    [HttpPut("UpdatePrice")]
    public IActionResult Update(UpdateProductPriceDto dto)
    {
        try{
            _warehouseService.UpdateProductPrice(dto);
            return NoContent();
        }
        catch(ProductNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch(Exception e)
        {
            return Conflict(e);
        } 
    }

    [HttpPut("UpdateAmount")]
    public IActionResult Update(UpdateProductAmountDto dto)
    {
        try{
            _warehouseService.UpdateProductAmount(dto);
            return NoContent();
        }
        catch(ProductNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch(Exception e)
        {
            return Conflict(e);
        } 
    }
    
    [HttpDelete]
    public IActionResult Delete(int id)
    {
        try{
            _warehouseService.DeleteProduct(id);
            return NoContent();
        }
        catch(Exception e)
        {
            return Conflict(e.Message);
        }
    }
}