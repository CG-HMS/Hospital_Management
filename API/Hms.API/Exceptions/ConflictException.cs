namespace Hms.API.Exceptions
{
    public class ConflictException : AppException
    {
        public ConflictException(string message) : base(409, message) { }
    }
}
