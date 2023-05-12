namespace AspTree.Exceptions
{
    public class SecureException: Exception
    {
        public SecureException(): base() { }

        public SecureException(string message) : base(message) { }
    }
}
