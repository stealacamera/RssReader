namespace RssReader.API.Common.DTOs.Requests;

public record EditUserRequest(string NewUsername);
public record ChangePasswordRequest(string OldPassword, string NewPassword);