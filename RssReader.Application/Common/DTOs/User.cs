namespace RssReader.Application.Common.DTOs;

public record User(int Id, string Email, string? Username = null);
public record LoggedInUser(User user, Tokens tokens);