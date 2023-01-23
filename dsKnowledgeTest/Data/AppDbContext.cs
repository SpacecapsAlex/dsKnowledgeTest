using dsKnowledgeTest.Models;
using Microsoft.EntityFrameworkCore;

namespace dsKnowledgeTest.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Test> Tests { get; set; }
    public DbSet<Faq> Faq { get; set; }
    public DbSet<Feedback> Feedbacks { get; set; }
    public DbSet<Question> Questions { get; set; }
}