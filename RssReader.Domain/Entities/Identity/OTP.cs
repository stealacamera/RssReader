using RssReader.Domain.Common;

namespace RssReader.Domain.Entities.Identity;

public class OTP : BaseEntity
{
    public int UserId { get; set; }
    public string Password { get; set; } = null!;

    public int RetryAttempts { get; set; }
    public DateTime ExpiryDate { get; set; }
}
