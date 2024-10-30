namespace RssReader.Application.Common.Exceptions.General
{
    public class ExistingEntityException : BaseException
    {
        public ExistingEntityException(string entity) : base($"{entity} already exists")
        {
        }
    }
}
