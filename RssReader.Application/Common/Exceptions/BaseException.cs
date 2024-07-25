namespace RssReader.Application.Common.Exceptions;

public abstract class BaseException : Exception
{
    public BaseException(string message) : base(message) { }
}
