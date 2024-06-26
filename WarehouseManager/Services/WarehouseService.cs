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
            if(dto.Name == string.Empty || dto.Name.Length < 2)
                throw new Exception($"Product name should be at least 2 signs.");
            if(dto.Price <= 0.00m)
                throw new Exception($"Product Price should be greater than 0.");
            if(dto.Amount <= 0)
                throw new Exception($"Product Amount should be greater than 0.");

            Product product = new Product();
            product.Name = dto.Name;
            product.Price = dto.Price;
            product.Amount = dto.Amount;
            product.UnitType = dto.UnitType.GetEnumValueByDescription<UnitEnum>();
            product.ProductType = dto.ProductType.GetEnumValueByDescription<ProductTypeEnum>();
            
            _warehouseRepository.AddProduct(product);
            return _productMapper.Map(product);
        }
        public void UpdateProduct(UpdateProductDto dto)
        {
            if(dto.Name == string.Empty || dto.Name.Length < 2)
                throw new Exception($"Product name should be at least 2 signs.");
            if(dto.Price <= 0.00m)
                throw new Exception($"Product Price should be greater than 0.");
            if(dto.Amount <= 0)
                throw new Exception($"Product Amount should be greater than 0.");

            var existingProduct = _warehouseRepository.GetProduct(dto.Id);
            if(existingProduct == null)
                throw new ProductNotFoundException($"Product not found! ID:{dto.Id}");
            Product product = new Product();
            product.Id = dto.Id;
            product.Name = dto.Name;
            product.Price = dto.Price;
            product.Amount = dto.Amount;
            product.UnitType = dto.UnitType.GetEnumValueByDescription<UnitEnum>();
            product.ProductType = dto.ProductType.GetEnumValueByDescription<ProductTypeEnum>();
            var mappedProduct = _mapper.Map<Product>(product);
            _warehouseRepository.UpdateProduct(mappedProduct);   
        }
        public void UpdateProductPrice(UpdateProductPriceDto dto)
        {
            var existingProduct = _warehouseRepository.GetProduct(dto.Id);
            if(existingProduct == null)
                throw new ProductNotFoundException($"Product not found! ID:{dto.Id}");
            if(dto.Price < 0.00M)
                throw new BadValueException($"Parameter price has incorrect value = {dto.Price} should be greater than zero.");
            
            existingProduct.Price = dto.Price;
            //var mappedProduct = _mapper.Map<Product>(existingProduct);
            _warehouseRepository.UpdateProduct(existingProduct);   
        }
        public void UpdateProductAmount(UpdateProductAmountDto dto)
        {
            var existingProduct = _warehouseRepository.GetProduct(dto.Id);
            if(existingProduct == null)
                throw new ProductNotFoundException($"Product not found! ID:{dto.Id}");

            existingProduct.Amount += dto.Amount;
            _warehouseRepository.UpdateProduct(existingProduct); 
        }
        public void DeleteProduct(int id)
        {
            var product = _warehouseRepository.GetProduct(id);
            if(product == null)
                throw new ProductNotFoundException($"Product not found! ID:{id}");
            _warehouseRepository.DeleteProduct(id);
        }
        public int CountProducts() => _warehouseRepository.GetProducts().Count();
        public bool Check(int productId, int productAmount)
        {
            var product = _warehouseRepository.GetProduct(productId);
            if(product == null)
                throw new ProductNotFoundException($"Product not found id:{productId}");

            if(productAmount <= product.Amount)
                return true;
            else return false;
        }
        public decimal GetProductPrice(int productId)
        {
            var product = _warehouseRepository.GetProduct(productId);
            if(product == null)
                throw new ProductNotFoundException($"Product not found id:{productId}");
            return product.Price;
        }
    }
}