using RssReader.Domain.Entities.Identity;

namespace RssReader.Application.Abstractions;

public interface IJwtProvider
{
    int ExtractIdFromToken(string token);

    string GenerateToken(int userId, string userEmail);
    string GenerateRefreshToken();

    void UpdateRefreshTokens(User user);
}