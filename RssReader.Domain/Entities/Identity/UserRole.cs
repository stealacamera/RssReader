using RssReader.Domain.Common;

namespace RssReader.Domain.Entities.Identity;

public class UserRole : BaseEntity
{
    public int UserId { get; set; }
    public int RoleId { get; set; }
}
