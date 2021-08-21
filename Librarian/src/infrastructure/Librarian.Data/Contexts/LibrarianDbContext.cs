using Librarian.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Librarian.Data.Contexts
{
    public class LibrarianDbContext
        : DbContext
    {
        public LibrarianDbContext(DbContextOptions<LibrarianDbContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
    }
}
