using WarehouseManager.Models;

namespace WarehouseManager.Dtos
{
    public class CreateProductDto
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Amount { get; set; }
        public UnitEnum UnitType{ get; set; }
        public ProductTypeEnum ProductType{ get; set; }
    }
}