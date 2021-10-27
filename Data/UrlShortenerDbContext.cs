using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using URLShortener.Areas.Identity.Data;
using URLShortener.Models;

namespace URLShortener.Data
{
    public class UrlShortenerDbContext : IdentityDbContext<User>
    {

        public UrlShortenerDbContext(DbContextOptions<UrlShortenerDbContext> options)
            : base(options)
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<UserUrl> UserUrls { get; set; }
        public DbSet<AnonUrl> AnonUrls { get; set; }


    }
}
