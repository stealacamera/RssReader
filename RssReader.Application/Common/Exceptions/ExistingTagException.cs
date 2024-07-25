namespace RssReader.Application.Common.Exceptions;

public class ExistingTagException : BaseException
{
    public ExistingTagException(string tagName) : base($"Tag '{tagName}' already exists")
    {
    }
}
