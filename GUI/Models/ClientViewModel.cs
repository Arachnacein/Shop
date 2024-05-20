namespace GUI.Models
{
    public class ClientViewModel
    {
        public Guid Id { get; set; }
        public int No { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime RegistryDate { get; set; }
    }
}