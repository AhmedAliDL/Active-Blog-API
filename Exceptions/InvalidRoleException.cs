namespace Active_Blog_Service.Exceptions
{
    public class InvalidRoleException : Exception
    {
        public InvalidRoleException(string message) : base(message)
        {

        }
        public InvalidRoleException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
