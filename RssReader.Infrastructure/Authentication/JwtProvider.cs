﻿using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RssReader.Application.Abstractions;
using RssReader.Domain.Entities;
using RssReader.Infrastructure.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace RssReader.Infrastructure.Authentication;

internal sealed class JwtProvider : IJwtProvider
{
    private readonly JwtOptions _options;

    public JwtProvider(IOptions<JwtOptions> options)
        => _options = options.Value;

    public int ExtractIdFromToken(string token)
    {
        var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);

        return int.Parse(jwtToken.Claims
                   .Single(e => e.Type == JwtRegisteredClaimNames.Sub)
                   .Value);
    }

    public string GenerateRefreshToken()
    {
        var randNr = new byte[64];
        
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randNr);
            return Convert.ToBase64String(randNr);
        }
    }

    public string GenerateToken(int userId, string userEmail)
    {
        // Create claim for user
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new(JwtRegisteredClaimNames.Email, userEmail)
        };

        // Create credentials
        var signingCredetials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            _options.Issuer,
            _options.Audience,
            claims,
            null,
            DateTime.UtcNow.AddMinutes(_options.TokenExpiration_Minutes),
            signingCredetials);

        string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
        return tokenValue;
    }

    public void UpdateRefreshTokens(User user)
    {
        user.RefreshToken = GenerateRefreshToken();
        user.RefreshTokenExpiry = DateTime.UtcNow.AddMinutes(_options.RefreshTokenExpiration_Minutes);
    }
}
