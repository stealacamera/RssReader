﻿namespace RssReader.Application.Common.DTOs;

public record Feed(int Id, string Url, string Name);

public record CreateFeedRequest(int FolderId, string Url, string? Name = null);