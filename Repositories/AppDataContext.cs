using LibraryAPI.Models.EntityModels;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Repositories
{
    public class AppDataContext : DbContext
    {
        public AppDataContext(DbContextOptions<AppDataContext> options) 
            : base(options) {}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Review>().HasKey(table => new {
                table.BookID, table.UserId
            });
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<Review> Reviews { get; set; }
        
    }
}