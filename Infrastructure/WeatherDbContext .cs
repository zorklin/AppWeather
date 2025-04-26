using AppWeather.Models;
using Microsoft.EntityFrameworkCore;

namespace AppWeather.Infrastructure
{
    public class WeatherDbContext : DbContext
    {
        public DbSet<Weather> Weather { get; set; }
        public DbSet<Admin> Admins { get; set; }

        public WeatherDbContext(DbContextOptions<WeatherDbContext> options)
            : base(options) { }

        protected override void ConfigureConventions(ModelConfigurationBuilder builder)
        {
            builder.Properties<DateOnly>()
                   .HaveConversion<DateOnlyConverter>()
                   .HaveColumnType("date");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Weather>()
                .HasIndex(w => w.Weather_Date)
                .IsUnique();
        }
    }
}
