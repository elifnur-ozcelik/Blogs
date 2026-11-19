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
        [Required]
        public string Title { get; set; }
        public string Content { get; set; }
        public decimal? Rating { get; set; }
        public DateTime PublishDate { get; set; }
        public bool IsArchived { get; set; }
        public string UserName { get; set; }
        public int? UserId { get; set; }
    }

    
    public class BlogUpdateHandler : DbService<Blog>,
        IRequestHandler<BlogUpdateRequest, CommandResponse>
    {
      
        public BlogUpdateHandler(DbContext db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(BlogUpdateRequest request,
            CancellationToken cancellationToken)
        {
            if (await DbQuery()
                .AnyAsync(blogEntity => blogEntity.Id != request.Id &&
                    blogEntity.Title == request.Title.Trim(),
                    cancellationToken))
                return Error($"{request.Title} blog exists!");
           
            var blogEntity = await DbSingleAsync(request.Id, cancellationToken);
            if (blogEntity is null)
                return Error("Blog not found!");

            blogEntity.Title = request.Title.Trim();
            blogEntity.Content = request.Content;
            blogEntity.Rating = request.Rating;
            blogEntity.PublishDate = request.PublishDate;
            blogEntity.IsArchived = request.IsArchived;
            blogEntity.UserName = request.UserName;
            blogEntity.UserId = request.UserId;

            await DbUpdateAsync(blogEntity, cancellationToken);

            return Success(blogEntity.Id,
                $"{blogEntity.Title} blog updated successfully.");
        }
    }
}
