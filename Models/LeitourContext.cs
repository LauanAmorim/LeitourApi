using Microsoft.EntityFrameworkCore;

namespace LeitourApi.Models
{
    public class LeitourContext : DbContext
    {
        public LeitourContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> User {get; set;}
        // public DbSet<Post> Post {get; set;}
    } 
}