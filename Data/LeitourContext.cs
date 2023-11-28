using Microsoft.EntityFrameworkCore;
using LeitourApi.Models;
using Microsoft.EntityFrameworkCore.Metadata;
namespace LeitourApi.Data
{
    public class LeitourContext : DbContext
    {
        public LeitourContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> User => Set<User>();
        public DbSet<Post> Posts { get; set; }
        public DbSet<Annotation> Annotations { get; set; }
        public DbSet<SavedBook> SavedBooks { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Post>().Property(e => e.UserName)
            .Metadata.SetBeforeSaveBehavior(PropertySaveBehavior.Ignore);
            modelBuilder.Entity<Post>().Property(e => e.UserName)
                .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

            modelBuilder.Entity<Post>().Property(e => e.Likes)
            .Metadata.SetBeforeSaveBehavior(PropertySaveBehavior.Ignore);
            modelBuilder.Entity<Post>().Property(e => e.Likes)
                .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            
            modelBuilder.Entity<Post>().Property(e => e.Liked)
            .Metadata.SetBeforeSaveBehavior(PropertySaveBehavior.Ignore);
            modelBuilder.Entity<Post>().Property(e => e.Liked)
                .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            
            modelBuilder
              .Entity<Post>()
              .ToView(PostRepository.VIEW_POST)
              .HasKey(t => t.Id);
            
            modelBuilder
              .Entity<Comment>()
              .ToView(CommentRepository.VIEW_COMMENT)
              .HasKey(t => t.CommentId);

              modelBuilder
              .Entity<User>()
              .ToView(UserRepository.VIEW_USER)
              .HasKey(t => t.Id);
           
            modelBuilder.Entity<Comment>().Property(e => e.UserName)
            .Metadata.SetBeforeSaveBehavior(PropertySaveBehavior.Ignore);
            modelBuilder.Entity<Comment>().Property(e => e.UserName)
                .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
        }
    }
}