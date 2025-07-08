using Microsoft.EntityFrameworkCore;
using Topic.QueryService.Domain.Entities;

namespace Topic.QueryService.Infrastructure.Data;

public class ApplicationContext(DbContextOptions<ApplicationContext> options) : DbContext(options)
{
    public DbSet<TopicEntity> Topics { get; set; }
    public DbSet<CommentEntity> Comments { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.LogTo(Console.WriteLine);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CommentEntity>()
            .HasOne<TopicEntity>()
            .WithMany(topic => topic.Comments)
            .HasForeignKey(comments => comments.TopicId)
            .HasConstraintName("FK_Comment_Topic_TopicId")
            .IsRequired();
    }
}