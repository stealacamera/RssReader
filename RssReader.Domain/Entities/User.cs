using RssReader.Domain.Common;

namespace RssReader.Domain.Entities;

public class User : BaseEntity
{
    public string Username { get; set; } = null!;
    public string HashedPassword { get; set; } = null!;

    public string Email { get; set; } = null!;
    public bool IsEmailConfirmed { get; set; }
    
    public DateTime CreatedAt {  get; set; }
}