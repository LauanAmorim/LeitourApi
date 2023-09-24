using Microsoft.EntityFrameworkCore;

namespace LeitourApi.Models
{
    public class LeitourContext : DbContext
    {

        public LeitourContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<WeatherForecast> Weather {get; set;}
        
    } 
}