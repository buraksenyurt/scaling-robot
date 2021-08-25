using Librarian.Application.Common.Interfaces;
using Librarian.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Librarian.Data.Contexts
{
    /*
     * DbContext nesnemiz. EF Core kullandığımız belli oluyor değil mi?
     * 
     * Şimdilik kitapların bir koleksiyonunu tutmakta.
     * 
     * IApplicationDbContext türetmesine de dikkat edelim. Core.Application katmanındaki sözleşmeyi kullanıyoruz.
     * Bu DI servislerine Entity Context nesnesini eklerken işimize yarayacak.
     * 
     */
    public class LibrarianDbContext
        : DbContext, IApplicationDbContext
    {
        public LibrarianDbContext(DbContextOptions<LibrarianDbContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
    }
}
