namespace Bunker.Domain.Shared.Exceptions;

public class InvalidGameOperationException : Exception
{
    public InvalidGameOperationException(string message)
        : base(message) { }
}
