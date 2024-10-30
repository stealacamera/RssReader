using RssReader.Domain.Entities.Identity;

namespace RssReader.Application.Abstractions.Repositories.Identity;

public interface IOTPsRepository : IBaseRepository<OTP>
{
    Task DeleteAllExpiredAsync(CancellationToken cancellationToken = default);
    Task<OTP?> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default);
    string GenerateOTP();
}
