using CORE.Domain;
using CORE.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Blogs.APP.Domain;

namespace Blogs.APP.Features.Blogs
{
    public class BlogQueryRequest : Record,
      IRequest<IQueryable<BlogQueryResponse>>
    {

    }

    public class BlogQueryResponse : Record
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public decimal? Rating { get; set; }
        public DateTime PublishDate { get; set; }
        public bool IsArchived { get; set; }
        public string UserName { get; set; }
        public int? UserId { get; set; }
    }

    public class BlogQueryHandler : DbService<Blog>,
       IRequestHandler<BlogQueryRequest, IQueryable<BlogQueryResponse>>
    {
        public BlogQueryHandler(DbContext db) : base(db)
        {
        }

        public Task<IQueryable<BlogQueryResponse>> Handle(
            BlogQueryRequest request, CancellationToken cancellationToken)
        {
           
            var query =
                DbQuery()
                .Select(blogEntity => new BlogQueryResponse()
                {
                    Id = blogEntity.Id,
                    Title = blogEntity.Title,
                    Content = blogEntity.Content,
                    Rating = blogEntity.Rating,
                    PublishDate = blogEntity.PublishDate,
                    IsArchived = blogEntity.IsArchived,
                    UserName = blogEntity.UserName,
                    UserId = blogEntity.UserId
                }); 

            return Task.FromResult(query);
        }
    }
}
