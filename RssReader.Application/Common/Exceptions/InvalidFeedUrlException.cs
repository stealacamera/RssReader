namespace RssReader.Application.Common.Exceptions;

public class InvalidFeedUrlException : BaseException
{
    public InvalidFeedUrlException() : base("Feed URL is invalid")
    {
    }
}
