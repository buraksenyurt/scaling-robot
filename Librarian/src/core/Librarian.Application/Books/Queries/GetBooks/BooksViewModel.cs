using Librarian.Application.Dtos.Books;
using System.Collections.Generic;

namespace Librarian.Application.Books.Queries.GetBooks
{
    /*
     * Kitap listesinin tamamını çeken Query'nin çalıştığı ViewModel nesnesi
     * 
     */
    public class BooksViewModel
    {
        public IList<BookDto> BookList { get; set; }
    }
}
