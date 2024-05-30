namespace UI.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Amount { get; set; }
        public Enums.UnitEnum UnitType{ get; set; }
        public Enums.ProductTypeEnum ProductType{ get; set; }
    }
}