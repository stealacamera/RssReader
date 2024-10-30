using RssReader.Domain.Common;

namespace RssReader.Domain.Entities.Identity;

public class RolePermission : BaseEntity
{
    public int RoleId { get; set; }
    public int PermissionId { get; set; }
}
