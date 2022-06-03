using Blog.DataContext.Mapings;
using Blog.Models;
using Microsoft.EntityFrameworkCore;

namespace Blog.DataContext
{
    //BlogDataContext vai representar minha tabela no banco
    public class BlogDataContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<User> Users { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder options)
          => options.UseSqlServer("Server=localhost,1433;Database=Blog;User ID=sa;Password=1q2w3e4r@#$;Trusted_Connection=False; TrustServerCertificate=True;");
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new MapinCategory());
            modelBuilder.ApplyConfiguration(new MapingPost());
            modelBuilder.ApplyConfiguration(new MapingUser());
        }

    }
}