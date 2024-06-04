namespace UI.Models
{
   public class ShowOrderViewModel
   {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string ProductName { get; set; }
        public int Amount { get; set; }
        public decimal Price { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime CompletionDate { get; set; }
        public bool Finished { get; set; }
    }
}