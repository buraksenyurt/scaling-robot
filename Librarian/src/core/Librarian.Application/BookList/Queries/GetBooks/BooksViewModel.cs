using Librarian.Application.Dtos.Books;
using System.Collections.Generic;

namespace Librarian.Application.BookList.Queries.GetBooks
{
    public class BooksViewModel
    {
        public IList<BookListDto> BookList { get; set; }
    }
}
