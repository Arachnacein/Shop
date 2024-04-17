namespace ClientManager.Dtos
{

public class ClientDto
{
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string? Surname { get; set; }

        public DateTime RegistryDate { get; set; }
}

}