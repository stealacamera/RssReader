namespace RssReader.Application.Common.Exceptions;

public class ExistingEmailException : BaseException
{
    public ExistingEmailException() : base("This email is already in use by another account")
    {
    }
}
