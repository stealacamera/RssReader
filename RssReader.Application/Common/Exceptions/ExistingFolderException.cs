namespace RssReader.Application.Common.Exceptions;

public class ExistingFolderException : BaseException
{
    public ExistingFolderException(string folderName) : base($"Folder '{folderName}' already exists")
    {
    }
}
