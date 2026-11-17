using System.ComponentModel.DataAnnotations;
using CORE.Domain;

namespace Blogs.APP.Domain;

public class Tag : Record
{
    [Required, StringLength(25)]
    public string Name { get; set; }

    public List<BlogTag> BlogTags { get; set; } = new();
}