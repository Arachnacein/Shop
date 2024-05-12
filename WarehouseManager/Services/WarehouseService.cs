using WarehouseManager.Models;
using WarehouseManager.Dtos;
using WarehouseManager.Data;
using AutoMapper;
using WarehouseManager.Exceptions;
using WarehouseManager.Mappers;
using Microsoft.IdentityModel.Tokens;

namespace WarehouseManager.Services
{
    public class WarehouseService : IWarehouseService
    {
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly IProductMapper _productMapper;
        private readonly IMapper _mapper;
        public WarehouseService(IWarehouseRepository warehouseRepository, IProductMapper productMapper, IMapper mapper)
        {
            _warehouseRepository = warehouseRepository;
            _productMapper = productMapper;
            _mapper = mapper;
        }
        public IEnumerable<ProductDto> GetAllProducts()
        {
            var products = _warehouseRepository.GetProducts();
            if(products.IsNullOrEmpty())
                throw new ArgumentNullException($"Not found any products!"); 
            return _productMapper.MapElements(products.ToList());
        }
        public ProductDto GetProduct(int id)
        {
            var product = _warehouseRepository.GetProduct(id);
            if(product == null)
                throw new ProductNotFoundException($"Product not found! ID:{id}");
            return _mapper.Map<ProductDto>(product);
        }
        public ProductDto AddNewProduct(CreateProductDto dto)
        {
            var mappedProduct = _mapper.Map<Product>(dto);
            _warehouseRepository.AddProduct(mappedProduct);
            return _productMapper.Map(mappedProduct);
        }
        public void UpdateProduct(UpdateProductDto dto)
        {
            var existingProduct = _warehouseRepository.GetProduct(dto.Id);
            if(existingProduct == null)
                throw new ProductNotFoundException($"Product not found! ID:{dto.Id}");
            var mappedProduct = _mapper.Map<Product>(dto);
            _warehouseRepository.UpdateProduct(mappedProduct);   
        }
        public void UpdateProductPrice(UpdateProductPriceDto dto)
        {
            var existingProduct = _warehouseRepository.GetProduct(dto.Id);
            if(existingProduct == null)
                throw new ProductNotFoundException($"Product not found! ID:{dto.Id}");
            if(dto.Price < 0.00M)
                throw new BadValueException($"Parameter price has incorrect value = {dto.Price} should be greater than zero.");
            
            var mappedProduct = _mapper.Map<Product>(dto);
            mappedProduct.Price = dto.Price;
            _warehouseRepository.UpdateProduct(mappedProduct);   
        }
        public void UpdateProductAmount(UpdateProductAmountDto dto)
        {
            var existingProduct = _warehouseRepository.GetProduct(dto.Id);
            if(existingProduct == null)
                throw new ProductNotFoundException($"Product not found! ID:{dto.Id}");
            if(dto.Amount < 0)
                throw new BadValueException($"Parameter amount has incorrect value = {dto.Amount} should be greater than zero.");
            
            var mappedProduct = _mapper.Map<Product>(dto);
            mappedProduct.Amount = dto.Amount;
            _warehouseRepository.UpdateProduct(mappedProduct); 
        }
        public void DeleteProduct(int id)
        {
            var product = _warehouseRepository.GetProduct(id);
            if(product == null)
                throw new ProductNotFoundException($"Product not found! ID:{id}");
            _warehouseRepository.DeleteProduct(id);
        }
        public int CountProducts() => _warehouseRepository.GetProducts().Count();
    }
}