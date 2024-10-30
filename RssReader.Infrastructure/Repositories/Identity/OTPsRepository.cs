using Microsoft.EntityFrameworkCore;
using RssReader.Application.Abstractions.Repositories.Identity;
using RssReader.Domain.Entities.Identity;
using System.Security.Cryptography;

namespace RssReader.Infrastructure.Repositories.Identity;

internal class OTPsRepository : BaseRepository<OTP>, IOTPsRepository
{
    public OTPsRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public string GenerateOTP()
    {
        byte[] randBytes = new byte[10], passwordBytes = new byte[4];

        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randBytes);                    // Generate random bytes
            int startingIndex = randBytes[^1] % 10;     // Get last digit of last element

            // Get 4 consecutive elements from array starting from found index
            for (int i = 0; i < 4; i++)
            {
                int newIndex = startingIndex + i;

                // Loop to beginning
                if (newIndex >= randBytes.Length)
                    newIndex -= randBytes.Length;

                passwordBytes[i] = randBytes[newIndex];
            }

            // Convert bytes to absolute integer
            var convertedNr = Math.Abs(BitConverter.ToInt32(passwordBytes, 0));
            var nrSubsection = convertedNr % 1000000;   // Get last 6 digits

            return nrSubsection.ToString();
        }
    }

    public async Task<OTP?> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default)
        => await _set.FindAsync([userId], cancellationToken: cancellationToken);

    public async Task DeleteAllExpiredAsync(CancellationToken cancellationToken = default)
    {
        var query = _set.Where(e => e.ExpiryDate < DateTime.UtcNow);
        await query.ExecuteDeleteAsync();
    }
}
