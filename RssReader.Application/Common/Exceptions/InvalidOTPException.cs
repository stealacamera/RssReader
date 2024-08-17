namespace RssReader.Application.Common.Exceptions;

public class InvalidOTPException : BaseException
{
    public InvalidOTPException() : base("OTP is expired or retry attempts have been exceeded")
    {
    }
}
