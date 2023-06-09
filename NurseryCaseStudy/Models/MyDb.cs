using Microsoft.EntityFrameworkCore;

namespace NurseryCaseStudy.Models
{
    public class MyDb : DbContext
    {
        public DbSet<Plant> plants { get; set; }
        public DbSet<Cart> carts { get; set; }
        public DbSet<Purchase> purchares { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("myDb");
        }
    }
}
