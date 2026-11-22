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
        
        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(150,
            ErrorMessage = "{0} must be maximum {1} characters!")]
        public string Title { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(2500,
            ErrorMessage = "{0} must be maximum {1} characters!")]
        public string Content { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(30, MinimumLength = 3,
            ErrorMessage = "{0} must be minimum {2} maximum {1} characters!")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [Range(0, 5, ErrorMessage = "{0} must be between {1} and {2}!")]
        public decimal? Rating { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        public int? UserId { get; set; }


        [Required(ErrorMessage = "At least one tag is required!")]
        public List<int> TagIds { get; set; }
    }

    public class BlogAddHandler : DbService<Blog>,
        IRequestHandler<BlogAddRequest, CommandResponse>
    {

        public BlogAddHandler(DbContext db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(
            BlogAddRequest request, CancellationToken cancellationToken)
        {
            if (await DbQuery().AnyAsync(b =>
                b.UserName == request.UserName.Trim(), cancellationToken
                ))
                return Error("Blog with the same user name exists!");

            var blogEntity = new Blog
            {
                Title = request.Title.Trim(),
                Content = request.Content.Trim(),
                UserName = request.UserName.Trim(),
                PublishDate = DateTime.Now,
                Rating = request.Rating ?? 0,
                UserId = request.UserId ?? 0,
                TagIds = request.TagIds
            };

            await DbAddAsync(blogEntity, cancellationToken);

            return Success(blogEntity.Id, "Blog created successfully.");
        }
    }
}
