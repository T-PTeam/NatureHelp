namespace Shared.Exceptions;

public class OperationCanNotBeCompleted : Exception
{
    public OperationCanNotBeCompleted(string message) : base("Operation can not be completed. The reason is that " + message)
    {
    }

    public OperationCanNotBeCompleted(string message, Exception innerException) : base("Operation can not be completed. The reason is that " + message, innerException)
    {
    }
}
