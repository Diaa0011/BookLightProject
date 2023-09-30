using learningProcess1razor.Models;
using Microsoft.EntityFrameworkCore;
namespace learningProcess1razor.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {
            
        }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                    new Category { Id = 1, Name = "Razor1", DisplayOrder = 1 },
                    new Category { Id = 2, Name = "Razor2", DisplayOrder = 2 },
                    new Category { Id = 3, Name = "Razor3", DisplayOrder = 3 }
                );
        }
    }
}
