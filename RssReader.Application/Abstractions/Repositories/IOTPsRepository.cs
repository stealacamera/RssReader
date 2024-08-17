using RssReader.Domain.Entities;

namespace RssReader.Application.Abstractions.Repositories;

public interface IOTPsRepository : IBaseRepository<OTP>
{
    Task DeleteAllExpiredAsync(CancellationToken cancellationToken = default);
    Task<OTP?> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default);
    string GenerateOTP();
}
