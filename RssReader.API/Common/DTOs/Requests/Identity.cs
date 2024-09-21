namespace RssReader.API.Common.DTOs.Requests;

public record LoginRequest(string Email, string Password);
public record SignupRequest(string Email, string Password, string? Username = null);

public record EmailVerificationRequest(int UserId, string OTP);

public record RefreshTokensRequest(string JwtToken, string RefreshToken);