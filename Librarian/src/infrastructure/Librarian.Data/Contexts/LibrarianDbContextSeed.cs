using Librarian.Domain.Entities;
using Librarian.Domain.Enums;
using System.Linq;
using System.Threading.Tasks;

namespace Librarian.Data.Contexts
{
    /*
     * Seed operasyonu için kullanılan statik sınıfımız.
     * Eğer hiç kitap yoksa birkaç tane ekleyecek.
     */
    public static class LibrarianDbContextSeed
    {
        public static async Task SeedDataAsync(LibrarianDbContext context)
        {
            if (!context.Books.Any())
            {
                await context.Books.AddAsync(new Book
                {
                    Title = "Kitab-Ül Hiyel",
                    Authors = "İsmail Oktay Anar",
                    Column = 2,
                    Row = 2,
                    Language = Language.Turkish,
                    Publisher = "İletişim Yayınları",
                    Status = Status.Read
                });
                await context.Books.AddAsync(new Book
                {
                    Title = "Sistem",
                    Authors = "James Ball",
                    Column = 1,
                    Row = 1,
                    Language = Language.Turkish,
                    Publisher = "Timaş Yayınları",
                    Status = Status.Read
                });
                await context.Books.AddAsync(new Book
                {
                    Title = "Outliers (Çizginin Dışındakiler)",
                    Authors = "Malcolm Gladwell",
                    Column = 5,
                    Row = 3,
                    Language = Language.Turkish,
                    Publisher = "MediaCat",
                    Status = Status.Read
                });
                await context.SaveChangesAsync();
            }
        }
    }
}
