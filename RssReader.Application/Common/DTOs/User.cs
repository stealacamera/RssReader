﻿namespace RssReader.Application.Common.DTOs;

public record User(int Id, string Email, string? Username = null);