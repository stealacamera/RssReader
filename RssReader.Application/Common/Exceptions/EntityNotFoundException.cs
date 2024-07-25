namespace RssReader.Application.Common.Exceptions;

public class EntityNotFoundException : BaseException
{
    public EntityNotFoundException(string entityType) : base($"{entityType} could not be found")
    {
    }
}
