using Librarian.Application.Books.Queries.ExportBooks;
using System.Collections.Generic;

namespace Librarian.Application.Common.Interfaces
{
    /*
     * CSV dosya çıktısı üreten servisin sözleşmesi
     * 
     * IEmailService tarafında olduğu gibi bu hizmeti de DI mekanizmasına kolayca dahil edebileceğiz.
     * 
     */
    public interface ICsvBuilder
    {
        byte[] BuildFile(IEnumerable<BookRecord> bookRecords);
    }
}
