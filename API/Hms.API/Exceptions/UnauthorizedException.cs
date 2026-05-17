namespace Hms.API.Exceptions
{
    public class UnauthorizedException : AppException
    {
        public UnauthorizedException(string message = "Unauthorized. Please log in.")
            : base(401, message) { }
    }
}
