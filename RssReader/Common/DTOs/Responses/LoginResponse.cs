namespace RssReader.API.Common.DTOs.Responses;

public record LoginResponse(string JwtToken, string RefreshToken);