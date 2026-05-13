namespace Hms.API.Exceptions;

/// <summary>
/// Exception thrown when model validation fails
/// </summary>
public class ValidationException : Exception
{
    public ValidationException(string message)
        : base(message)
    {
        Errors = new Dictionary<string, string[]>();
    }
}