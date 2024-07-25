namespace RssReader.Application.Common.Exceptions;

public class UnauthorizedException : BaseException
{
    public UnauthorizedException() : base("Not authorized to complete request")
    {
    }
}
