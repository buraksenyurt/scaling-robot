using System;

namespace Librarian.Application.Common.Exceptions
{
    public class BookNotFoundException
        : Exception
    {
        public BookNotFoundException(int bookId)
          : base($"{bookId} nolu kitap envanterde bulunamadı") { }
    }
}
