using Librarian.Application.Common.Mappings;
using Librarian.Domain.Entities;

namespace Librarian.Application.Dtos.Books
{
    public class BookListDto
        : IMapFrom<Book>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Authors { get; set; }
        public string Publisher { get; set; }
    }
}
