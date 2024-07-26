namespace RssReader.API.Common.DTOs;

public record LoginRequest(string Email, string Password);
public record SignupRequest(string Email, string Password, string? Username = null);

public record EditUserRequest(string NewUsername);
public record ChangePasswordRequest(string OldPassword, string NewPassword);