using System.ComponentModel.DataAnnotations;

namespace WarehouseManager.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        
        [MinLength(2)]
        [MaxLength(30)]
        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int Amount { get; set; }

        [Required]
        public UnitEnum UnitType{ get; set; }

        [Required]
        public ProductTypeEnum ProductType{ get; set; }
    }
}