using CORE.Domain;
using CORE.Models;
using CORE.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Blogs.APP.Domain;


namespace Blogs.APP.Features.Blogs
{
    public class BlogAddRequest : Record, IRequest<CommandResponse>
    {
        [Required]
        public string Title { get; set; }
        public string Content { get; set; }
        public decimal? Rating { get; set; }
        public DateTime PublishDate { get; set; }
        public bool IsArchived { get; set; }
        public string UserName { get; set; }
        public int? UserId { get; set; }

    }

    public class BlogAddHandler : DbService<Blog>,
        IRequestHandler<BlogAddRequest, CommandResponse>
    {
     
        public BlogAddHandler(DbContext db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(BlogAddRequest request,
            CancellationToken cancellationToken)
        {
            if (await DbQuery()
                .AnyAsync(blogEntity =>
                    blogEntity.Title == request.Title.Trim(),
                        cancellationToken))
                return Error(request.Title + " blog exists!");
         
            var blogEntity = new Blog
            {
                Title = request.Title.Trim(),
                Content = request.Content,
                Rating = request.Rating,
                PublishDate = request.PublishDate,
                IsArchived = request.IsArchived,
                UserName = request.UserName,
                UserId = request.UserId
            };

            await DbAddAsync(blogEntity, cancellationToken);

            return Success(blogEntity.Id, blogEntity.Title +
                " blog created successfully.");
        }
    }
}
