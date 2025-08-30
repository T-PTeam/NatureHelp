namespace Domain.Exceptions;

public class EntityNotFoundException<T> : Exception
{
    public EntityNotFoundException()
        : base($"Entity with type {nameof(T)} not found.") { }

    public EntityNotFoundException(string message)
        : base(message) { }

    public EntityNotFoundException(string message, Exception innerException)
        : base(message, innerException) { }
}