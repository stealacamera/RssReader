namespace RssReader.Application.Common.Exceptions;

public class InvalidLoginCredentialsException : BaseException
{
    public InvalidLoginCredentialsException() : base("Invalid email and/or password")
    {
    }
}
