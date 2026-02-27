namespace Template.Library.Exceptions
{
    public class GeneralException : Exception
    {
        public GeneralException() : base() { }
        public GeneralException(string message) : base(message) { }
        public GeneralException(string message, Exception inner) : base(message, inner) { }
    }
}
