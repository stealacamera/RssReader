namespace RssReader.Application.Common.Exceptions.General;

public class UnauthorizedException : BaseException
{
    public UnauthorizedException() : base("Not authorized to complete request")
    {
    }
}
