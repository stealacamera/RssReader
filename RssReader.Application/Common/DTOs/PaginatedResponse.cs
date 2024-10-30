namespace RssReader.Application.Common.DTOs;

public record PaginatedResponse<TCursor, TValue>(
    TCursor? NextCursor, 
    TValue Items)
    where TCursor : struct;