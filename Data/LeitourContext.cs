using Microsoft.EntityFrameworkCore;
using LeitourApi.Models;
namespace LeitourApi.Data
{
    public class LeitourContext : DbContext
    {
        public LeitourContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> User => Set<User>();
        public DbSet<FollowUser> FollowUsers { get; set; }
        //public DbSet<FollowingPage> FollowingPages { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<Annotation> Annotations { get; set; }
        public DbSet<SavedBook> SavedBooks { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            


        }
    }
}