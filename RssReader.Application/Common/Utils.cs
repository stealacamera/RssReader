using CodeHollow.FeedReader.Parser;
using RssReader.Application.Common.Exceptions;

namespace RssReader.Application.Common;

internal static class Utils
{
    public static async Task<Domain.Entities.Feed> CreateFeedEntity(string url, string? name = null, CancellationToken cancellationToken = default)
    {
        try
        {
            var feedResult = await CodeHollow.FeedReader
                                             .FeedReader
                                             .ReadAsync(url, cancellationToken);

            return new Domain.Entities.Feed
            {
                CreatedAt = DateTime.UtcNow,
                Url = url,
                Name = name ?? feedResult.Title,
                IconUrl = string.IsNullOrWhiteSpace(feedResult.ImageUrl) ? null : feedResult.ImageUrl
            };
        }
        catch (Exception ex) when (ex is InvalidFeedLinkException || ex is FeedTypeNotSupportedException)
        {
            throw new InvalidFeedUrlException();
        }
    }
}
