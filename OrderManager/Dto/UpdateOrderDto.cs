namespace OrderManager.Dto
{
    public class UpdateOrderDto
    {
        public int Id { get; set; }
        public Guid Id_User { get; set; }
        public int Id_Product { get; set; }
        public int Amount { get; set; }
        public decimal Price { get; set; }
        public DateTime CompletionDate { get; set; }
        public bool Finished { get; set; }
    }
}