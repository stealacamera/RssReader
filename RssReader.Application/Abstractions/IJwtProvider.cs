namespace RssReader.Application.Abstractions;

public interface IJwtProvider
{
    int ExtractIdFromToken(string token);

    string GenerateToken(int userId, string userEmail);
    string GenerateRefreshToken();

    void UpdateRefreshTokens(Domain.Entities.User user);
}