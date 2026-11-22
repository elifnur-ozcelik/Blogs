using CORE.Domain;
using CORE.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Blogs.APP.Domain;
using Blogs.APP.Features.Tags;

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

       
        public string IsArchivedR { get; set; }
        public string RatingR { get; set; }
        public string PublishDateR { get; set; }
      
        public string TagNamesR { get; set; }

        public List<TagQueryResponse> TagsR { get; set; }
    }

    public class BlogQueryHandler : DbService<Blog>,
        IRequestHandler<BlogQueryRequest,
            IQueryable<BlogQueryResponse>>
    {

        public BlogQueryHandler(DbContext db) : base(db)
        {
        }

        protected override IQueryable<Blog> DbQuery()
        {

            return base.DbQuery()
                .Include(b => b.BlogTags) 
                .ThenInclude(bt => bt.Tag) 
                .OrderByDescending(b => (double?)b.Rating)
                .ThenByDescending(b => b.PublishDate)
                .ThenBy(b => b.UserName);
        }
        public Task<IQueryable<BlogQueryResponse>> Handle(
            BlogQueryRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(
                DbQuery().Select(b => new BlogQueryResponse
                {
                    Id = b.Id,
                    Title = b.Title,
                    Content = b.Content,
                    Rating = b.Rating,
                    PublishDate = b.PublishDate,
                    IsArchived = b.IsArchived,
                    UserName = b.UserName,
                    UserId = b.UserId,

                    TagNamesR = string.Join(", ", b.BlogTags
                        .OrderBy(bt => bt.Tag.Name)
                        .Select(bt => bt.Tag.Name)),


                    TagsR = b.BlogTags.OrderBy(bt => bt.Tag.Name)
                        .Select(bt => new TagQueryResponse
                        {

                            Id = bt.Tag.Id,
                            Name = bt.Tag.Name
                        }).ToList(),

          
                    PublishDateR = b.PublishDate
                        .ToString("MM/dd/yyyy HH:mm.ss"),

                    IsArchivedR = b.IsArchived ? "Yes" : "No",

                    RatingR = b.Rating.HasValue ?
                        b.Rating.Value.ToString("N1") : string.Empty
                }));
        }
    }
}
