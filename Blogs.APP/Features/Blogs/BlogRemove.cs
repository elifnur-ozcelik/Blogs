using CORE.Domain;
using CORE.Models;
using CORE.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Blogs.APP.Domain;


namespace Blogs.APP.Features.Blogs
{
    public class BlogRemoveRequest : Record, IRequest<CommandResponse>
    {
    }

    public class BlogRemoveHandler : DbService<Blog>,
        IRequestHandler<BlogRemoveRequest, CommandResponse>
    {
        public BlogRemoveHandler(DbContext db) : base(db)
        {
        }
        public async Task<CommandResponse> Handle(BlogRemoveRequest request,
            CancellationToken cancellationToken)
        {
            var blogEntity = await DbSingleAsync(request.Id, cancellationToken);
            if (blogEntity is null)
                return Error("Blog not found!");

            await DbRemoveAsync(blogEntity, cancellationToken);

            return Success(blogEntity.Id,
                $"{blogEntity.Title} blog deleted successfully.");
        }
    }
}
