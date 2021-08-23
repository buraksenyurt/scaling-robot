using Librarian.Application.Common.Mappings;
using Librarian.Domain.Entities;

namespace Librarian.Application.BookList.Queries.ExportBooks
{
    /*
     * CSV içerisine hangi kitap bilgilerini basacağımızı tutan basit bir tip.
     * 
     * Bir nesne dönüşümü söz konusu olduğundan IMapFrom<T> uyarlaması var.
     * 
     */
    public class BookRecord
        :IMapFrom<Book>
    {
        public string Title { get; set; }
        public string Authors { get; set; }
        public string Publisher { get; set; }
    }
}
