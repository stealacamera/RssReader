using FluentValidation;
using MediatR;
using RssReader.Application.Common;
using RssReader.Application.Common.DTOs;
using RssReader.Application.Common.Exceptions;
using RssReader.Domain.Abstractions;

namespace RssReader.Application.Behaviour.Tags.Queries.GetAllForUser;

internal class GetAllTagsForUserQueryHandler : BaseCommandHandler, IRequestHandler<GetAllTagsForUserQuery, IList<Tag>>
{
    public GetAllTagsForUserQueryHandler(IWorkUnit workUnit) : base(workUnit)
    {
    }

    public async Task<IList<Tag>> Handle(GetAllTagsForUserQuery request, CancellationToken cancellationToken)
    {
        // Validate request properties
        await new GetAllTagsForUserQueryValidator().ValidateAndThrowAsync(request, cancellationToken);

        // Validate user
        if (!await _workUnit.UsersRepository
                            .DoesInstanceExistAsync(request.RequesterId, cancellationToken))
            throw new UnauthorizedException();

        // Get user tags
        var tags = await _workUnit.TagsRepository
                                  .GetAllForUserAsync(request.RequesterId, cancellationToken);

        return tags.Select(e => new Tag(e.Id, e.Name))
                   .ToList();
    }
}
