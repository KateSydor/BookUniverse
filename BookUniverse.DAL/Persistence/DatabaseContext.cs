using BookUniverse.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookUniverse.DAL.Persistence
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Book> Book { get; set; }
        public DbSet<BookFolder> BookFolder { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Folder> Folder { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<UserBook> UserBook { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {

        }
    }
}
