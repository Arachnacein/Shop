using Microsoft.AspNetCore.Mvc;
using WarehouseManager.Dtos;
using WarehouseManager.Exceptions;
using WarehouseManager.Services;

namespace WarehouseManager.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IWarehouseService _warehouseService;
        public ProductController(IWarehouseService warehouseService)
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

        [HttpGet("GetPrice/{id}")]
        public IActionResult GetPrice(int id)
        {
            try{
                var productPrice = _warehouseService.GetProductPrice(id);
                return Ok(productPrice);
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
                return Created($"api/products/{product.Id}", product);
            }
            catch(Exception e)
            {
                return Conflict(e.Message);
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
                return Conflict(e.Message);
            } 
        }

        [HttpPut("UpdatePrice")]
        public IActionResult UpdatePrice(UpdateProductPriceDto dto)
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
        public IActionResult UpdateAmount(UpdateProductAmountDto dto)
        {
            try{
                _warehouseService.UpdateProductAmount(dto);
                return NoContent();
            }
            catch(ProductNotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
            catch(Exception e)
            {
                return Conflict(new { message = e.Message });
            } 
        }
        
        [HttpDelete("{id}")]
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

        [HttpGet("Check")]
        public IActionResult Check([FromQuery]int productId, [FromQuery]int productAmount)
        {
            try{
                var result = _warehouseService.Check(productId, productAmount);
                if(result)
                    return Ok(true);
                else return Ok(false);
            }
            catch(ProductNotFoundException e)
            {
                return Conflict(e.Message);
            }
            catch(Exception e)
            {
                return Conflict(e.Message);
            }
        }
    }
}