using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CORE.Domain;

namespace Blogs.APP.Domain;

public class Blog : Record
{
    [Required, StringLength(150)]
    public string Title { get; set; }
    
    [Required, StringLength(2500)]
    public string Content  { get; set; }
    
    public decimal? Rating { get; set; }
    
    [Required]
    public DateTime PublishDate { get; set; }
    
    [Required]
    public bool IsArchived { get; set; }
    
    [Required, StringLength(30)]
    public string UserName { get; set; }
    
    [Required]
    public int? UserId { get; set; }

    public List<BlogTag> BlogTags { get; set; } = new();

    [NotMapped]
    public List<int> TagIds
    {
        get => BlogTags.Select(blogTagEntity => blogTagEntity.TagId).ToList();
        
        set => BlogTags = value.Select(tagIdValue => new BlogTag
        {
            TagId = tagIdValue
        }).ToList();
    }
    
    public string RefreshToken { get; set; }
    
    public DateTime? RefreshTokenExpiration { get; set; }
}