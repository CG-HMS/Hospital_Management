namespace Hms.API.Exceptions
{
    public class NotFoundException : AppException
    {
        public NotFoundException(string message) : base(message, 404) { }
        public NotFoundException(string entity, object key)
            : base( $"{entity} with key '{key}' was not found.", 404) { }

    }
}
