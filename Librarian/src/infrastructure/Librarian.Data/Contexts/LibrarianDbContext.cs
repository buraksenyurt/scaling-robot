using Librarian.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Librarian.Data.Contexts
{
    /*
     * DbContext nesnemiz. EF Core kullandığımız belli oluyor değil mi?
     * 
     * Şimdilik kitapların bir koleksiyonunu tutmakta.
     * 
     */
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
