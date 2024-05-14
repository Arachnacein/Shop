using System.ComponentModel.DataAnnotations;

namespace OrderManager.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Id_Product { get; set; }

        [Required]
        public Guid Id_User { get; set; }

        [Required]
        public int Amount { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

        [Required]
        public DateTime CompletionDate { get; set; }

        [Required]
        public bool Finished { get; set; }
    }
}