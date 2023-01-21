using Microsoft.EntityFrameworkCore;

namespace dsKnowledgeTest.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
        Database.EnsureCreated();
    }
}