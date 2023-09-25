using Microsoft.EntityFrameworkCore;

namespace LeitourApi.Models
{
    public class LeitourContext : DbContext
    {
        public LeitourContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> User => Set<User>();
        // public DbSet<Post> Post {get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}