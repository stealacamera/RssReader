using FluentValidation;
using MediatR;
using RssReader.Application.Abstractions;
using RssReader.Application.Common;
using RssReader.Application.Common.DTOs;

namespace RssReader.Application.Behaviour.Operations.Tags.Queries.GetAllForUser;

internal class GetAllTagsForUserQueryHandler : BaseHandler, IRequestHandler<GetAllTagsForUserQuery, IList<Tag>>
{
    public GetAllTagsForUserQueryHandler(IWorkUnit workUnit) : base(workUnit)
    {
    }

    public async Task<IList<Tag>> Handle(GetAllTagsForUserQuery request, CancellationToken cancellationToken)
    {
        // Validate request properties
        await new GetAllTagsForUserQueryValidator().ValidateAndThrowAsync(request, cancellationToken);
        await ValidateRequesterAsync(request.RequesterId, cancellationToken);

        // Get user tags
        var tags = await _workUnit.TagsRepository
                                  .GetAllForUserAsync(request.RequesterId, cancellationToken);

        return tags.Select(e => new Tag(e.Id, e.Name))
                   .ToList();
    }
}
