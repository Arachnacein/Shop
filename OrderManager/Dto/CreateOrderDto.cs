namespace OrderManager.Dto
{
    public class CreateOrderDto
    {
        public Guid Id_User { get; set; }
        public int Id_Product { get; set; }
        public int Amount { get; set; }
    }
}