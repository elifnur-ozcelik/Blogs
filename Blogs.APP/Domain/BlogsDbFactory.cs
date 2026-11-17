using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Blogs.APP.Domain;

public class BlogsDbFactory : IDesignTimeDbContextFactory<BlogsDb>
{
    const string CONNECTIONSTRING = "data source=BlogsDB.db";

    public BlogsDb CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<BlogsDb>();
        optionsBuilder.UseSqlite(CONNECTIONSTRING);
        return new BlogsDb(optionsBuilder.Options);
    }

    public void SeedDb()
    {
        var db = CreateDbContext(null);
        
        var BlogTags = db.BlogTags.ToList();
        db.BlogTags.RemoveRange(BlogTags);
        var Blogs = db.Blogs.ToList();
        db.Blogs.RemoveRange(Blogs);
        var Tags = db.Tags.ToList();
        db.Tags.RemoveRange(Tags);

        db.Database.ExecuteSqlRaw("UPDATE SQLITE_SEQUENCE " + "SET SEQ=0 WHERE NAME='BlogTags'");
        db.Database.ExecuteSqlRaw("UPDATE SQLITE_SEQUENCE " + "SET SEQ=0 WHERE NAME='Blogs'");
        db.Database.ExecuteSqlRaw("UPDATE SQLITE_SEQUENCE " + "SET SEQ=0 WHERE NAME='Tags'");

    }
}