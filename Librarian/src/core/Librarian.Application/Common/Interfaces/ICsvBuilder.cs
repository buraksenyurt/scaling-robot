using Librarian.Application.BookList.Queries.ExportBooks;
using System.Collections.Generic;

namespace Librarian.Application.Common.Interfaces
{
    public interface ICsvBuilder
    {
        byte[] BuildFile(IEnumerable<BookRecord> bookRecords);
    }
}
