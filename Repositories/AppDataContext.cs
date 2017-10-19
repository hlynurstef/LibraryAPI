using LibraryAPI.Models.EntityModels;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Repositories
{
    public class AppDataContext : DbContext
    {
        public AppDataContext(DbContextOptions<AppDataContext> options) 
            : base(options) {}

        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }
    }
}