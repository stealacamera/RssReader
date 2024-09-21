namespace RssReader.Application.Common.Exceptions.General;

public class EntityNotFoundException : BaseException
{
    public EntityNotFoundException(string entityType) : base($"{entityType} could not be found")
    {
    }
}
