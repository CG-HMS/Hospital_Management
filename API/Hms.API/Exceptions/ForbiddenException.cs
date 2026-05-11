namespace Hms.API.Exceptions
{
    public class ForbiddenException : AppException
    {
        public ForbiddenException(string message = "Forbidden. You don't have permission to to perform this action.")
            : base(message, 403) { }
        
    }
}
