using CORE.Domain;
using CORE.Models;
using CORE.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Blogs.APP.Domain;


namespace Blogs.APP.Features.Blogs
{
    public class BlogUpdateRequest : Record, IRequest<CommandResponse>
    {
        [Required, StringLength(150)]
        public string Title { get; set; }

        [Required, StringLength(2500)]
        public string Content { get; set; }

        [Required, StringLength(30, MinimumLength = 3)]
        public string UserName { get; set; }

        [Required, Range(0, 5)]
        public decimal? Rating { get; set; }

        [Required]
        public int? UserId { get; set; }

        [Required]
        public List<int> TagIds { get; set; }
    }

    public class BlogUpdateHandler : DbService<Blog>,
        IRequestHandler<BlogUpdateRequest, CommandResponse>
    {

        public BlogUpdateHandler(DbContext db) : base(db)
        {
        }

        protected override IQueryable<Blog> DbQuery()
        {
            return base.DbQuery().Include(b => b.BlogTags);
        }

        public async Task<CommandResponse> Handle(
            BlogUpdateRequest request,
            CancellationToken cancellationToken)
        {
            if (await DbQuery().AnyAsync(b => b.Id != request.Id &&
                b.UserName == request.UserName.Trim(), cancellationToken))
                return Error("Blog with the same user name exists!");

            var blogEntity = await DbSingleAsync(request.Id, cancellationToken);
            if (blogEntity is null)
                return Error("Blog not found!");

            if (request.TagIds is not null)
                DbRemove(blogEntity.BlogTags);

            blogEntity.Title = request.Title.Trim();
            blogEntity.Content = request.Content.Trim();
            blogEntity.UserName = request.UserName.Trim();

            blogEntity.Rating = request.Rating ?? 0;
            blogEntity.UserId = request.UserId ?? 0;

            blogEntity.TagIds = request.TagIds;

            await DbUpdateAsync(blogEntity, cancellationToken);

            return Success(blogEntity.Id, "Blog updated successfully!");
        }
    }
}
