using CORE.Domain;
using CORE.Models;
using CORE.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Blogs.APP.Domain;


namespace Blogs.APP.Features.Tags
{
    public class TagAddRequest : Record, IRequest<CommandResponse>
    {
        [Required, StringLength(25)]
        public string Name { get; set; }
    }

    public class TagAddHandler : DbService<Tag>,
        IRequestHandler<TagAddRequest, CommandResponse>
    {
        public TagAddHandler(DbContext db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(TagAddRequest request,
            CancellationToken cancellationToken)
        {
            if (await DbQuery()
                .AnyAsync(tagEntity =>
                    tagEntity.Name == request.Name.Trim(),
                        cancellationToken))
                return Error(request.Name + " tag exists!");
           
            var tagEntity = new Tag
            {
                Name = request.Name.Trim()
            };

            await DbAddAsync(tagEntity, cancellationToken);

            return Success(tagEntity.Id, tagEntity.Name +
                " tag created successfully.");
        }
    }
}
