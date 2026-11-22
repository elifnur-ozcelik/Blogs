using CORE.Domain;
using CORE.Models;
using CORE.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Blogs.APP.Domain;


namespace Blogs.APP.Features.Tags
{
    public class TagUpdateRequest : Record, IRequest<CommandResponse>
    {
        
        [Required, StringLength(25)]
        public string Name { get; set; }

    }

    public class TagUpdateHandler : DbService<Tag>,
        IRequestHandler<TagUpdateRequest, CommandResponse>
    {
        public TagUpdateHandler(DbContext db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(TagUpdateRequest request,
            CancellationToken cancellationToken)
        {
            if (await DbQuery()
                .AnyAsync(tagEntity => tagEntity.Id != request.Id &&
                    tagEntity.Name == request.Name.Trim(),
                    cancellationToken))
                return Error($"{request.Name} tag exists!");
            
            var tagEntity = await DbSingleAsync(request.Id, cancellationToken);
            if (tagEntity is null)
                return Error("Tag not found!");

            tagEntity.Name = request.Name.Trim();

            await DbUpdateAsync(tagEntity, cancellationToken);

            return Success(tagEntity.Id,
                $"{tagEntity.Name} tag updated successfully.");
        }
    }
}
