using CORE.Domain;

namespace Blogs.APP.Domain;

public class BlogTag : Record
{
    public int BlogId { get; set; }
    public Blog Blog { get; set; }
    public int TagId { get; set; }
    public Tag Tag { get; set; }
}