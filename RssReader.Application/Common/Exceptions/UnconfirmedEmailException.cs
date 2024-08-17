namespace RssReader.Application.Common.Exceptions;

public class UnconfirmedEmailException : BaseException
{
    public UnconfirmedEmailException() : base("Email is unconfirmed")
    {
    }
}
