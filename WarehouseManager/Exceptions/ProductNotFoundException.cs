namespace WarehouseManager.Exceptions
{
    public class ProductNotFoundException : Exception
    {
        public ProductNotFoundException(){}
        public ProductNotFoundException(string msg) : base(msg)
        {
        }
    }

}