using CORE.Domain;
using CORE.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Blogs.APP.Domain;

namespace Blogs.APP.Features.Tags
{
    public class TagQueryRequest : Record,
        IRequest<IQueryable<TagQueryResponse>>
    {
    }

    public class TagQueryResponse : Record
    {
        public string Name { get; set; }
    }

    public class TagQueryHandler : DbService<Tag>,
        IRequestHandler<TagQueryRequest, IQueryable<TagQueryResponse>>
    {
        public TagQueryHandler(DbContext db) : base(db)
        {
        }

        public Task<IQueryable<TagQueryResponse>> Handle(
            TagQueryRequest request, CancellationToken cancellationToken)
        {
            var query =
                DbQuery()
                .Select(tagEntity => new TagQueryResponse()
                {
                    Id = tagEntity.Id,
                    Name = tagEntity.Name
                }); 
            
            return Task.FromResult(query);
        }
    }
}
