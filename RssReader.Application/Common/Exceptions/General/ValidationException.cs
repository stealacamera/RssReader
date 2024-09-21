namespace RssReader.Application.Common.Exceptions.General;

public class ValidationException : Exception
{
    public IDictionary<string, string[]> ErrorDetails { get; }

    public ValidationException(IDictionary<string, string[]> errorDetails)
        => ErrorDetails = errorDetails;
}
