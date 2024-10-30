using MediatR;

namespace RssReader.Application.Behaviour.Operations.Feeds.Commands.PullAll;

public record PullFeedsCommand : IRequest;