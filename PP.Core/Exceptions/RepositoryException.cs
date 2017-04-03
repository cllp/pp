namespace PP.Core.Exceptions
{
    class RepositoryException : ExceptionBase
    {
        public RepositoryException(string message, string data)
            : base(message)
        {
            Data = data;
        }
    }
}
