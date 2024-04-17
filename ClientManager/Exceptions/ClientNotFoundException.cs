namespace ClientManager.Exceptions
{
    public class ClientNotFoundException : Exception
    {
        public ClientNotFoundException(string msg) : base(msg)
        {}
    }
}