using dsKnowledgeTest.Models;
using Microsoft.EntityFrameworkCore;

namespace dsKnowledgeTest.Data;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public AppDbContext(DbContextOptions options) : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Test> Tests { get; set; }
}