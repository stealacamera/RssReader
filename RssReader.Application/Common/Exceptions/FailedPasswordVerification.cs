namespace RssReader.Application.Common.Exceptions;

public class FailedPasswordVerification : BaseException
{
    public FailedPasswordVerification() : base("Password does not match current password")
    {
    }
}
