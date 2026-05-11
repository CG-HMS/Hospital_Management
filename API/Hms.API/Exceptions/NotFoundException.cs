namespace Hms.API.Exceptions
{
    public class NotFoundException : AppException
    {
        public NotFoundException(string message) : base(404, message) { }
        public NotFoundException(string entity, object key)
            : base(404, $"{entity} with key '{key}' was not found.") { }
    }
}
