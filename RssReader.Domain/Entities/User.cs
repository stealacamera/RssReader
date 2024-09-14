using RssReader.Domain.Common;

namespace RssReader.Domain.Entities;

public class User : BaseSimpleEntity
{
    public string? Username { get; set; }
    public string HashedPassword { get; set; } = null!;

    public string Email { get; set; } = null!;
    public bool IsEmailConfirmed { get; set; }
    
    public string? RefreshToken {  get; set; }
    public DateTime RefreshTokenExpiry {  get; set; }
}