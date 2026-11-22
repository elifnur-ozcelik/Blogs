using CORE.Domain;
using CORE.Models;
using CORE.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Blogs.APP.Domain;


namespace Blogs.APP.Features.Tags
{
    public class TagRemoveRequest : Record, IRequest<CommandResponse>
    {
    }

   
    public class TagRemoveHandler : DbService<Tag>,
        IRequestHandler<TagRemoveRequest, CommandResponse>
    {
        
        public TagRemoveHandler(DbContext db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(TagRemoveRequest request,
            CancellationToken cancellationToken)
        {
            
            var tagEntity = await DbSingleAsync(request.Id, cancellationToken);
            if (tagEntity is null)
                return Error("Tag not found!");

           
            await DbRemoveAsync(tagEntity, cancellationToken);

            return Success(tagEntity.Id,
                $"{tagEntity.Name} tag deleted successfully.");
        }
    }
}
