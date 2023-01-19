using Microsoft.EntityFrameworkCore;

namespace NewsBlog.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<NewsModel> DbNews { get; set; } = null!;
        public DbSet<NewsImageModel> DbNewsImages { get; set; } = null!;
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { /*Database.EnsureDeleted(); Database.EnsureCreated();*/ }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        { optionsBuilder.UseSqlServer(); }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        { base.OnModelCreating(modelBuilder); }
    }
}
