using System.ComponentModel.DataAnnotations;

namespace ClientManager.Models
{
    public class Client
    {
        [Key]
        public Guid Id { get; set; }

        [MinLength(2)]
        [MaxLength(30)]
        [Required]
        public string? Name { get; set; }

        [MinLength(4)]
        [MaxLength(45)]
        [Required]
        public string? Surname { get; set; }

        public DateTime RegistryDate { get; set; }



    }
}

