using WarehouseManager.Models;

namespace WarehouseManager.Dtos
{
    public class CreateProductDto
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Amount { get; set; }
        public string UnitType{ get; set; }
        public string ProductType{ get; set; }
    }
}